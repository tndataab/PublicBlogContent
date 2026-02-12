using Anthropic;
using Anthropic.Models.Messages;
using Spectre.Console;
using System.Text.Json;

// Sample prompt:
// Create a simple fizz buzz application in C#, with program.cs and project file. Do use .NET 10.

var workFolder = @"c:\MyAgent";
Directory.CreateDirectory(workFolder);

var agent = new MySmartAgent(workFolder);

// ========== THE AGENTIC LOOP ==========

while (true)
{
    var input = GetUserInput();

    var response = await agent.SendMessage(input);

    // LLM chains tools together (read file -> edit file -> test code)
    while (agent.WantsToUseTool(response))
        response = await agent.ProcessToolCalls(response);

}

// ======================================

string GetUserInput()
{
    Console.Write("\nWhat do you want me to do: ");
    var input = Console.ReadLine();
    return input ?? "";
}

public class MySmartAgent
{
    private AnthropicClient client = new();
    private List<MessageParam> context = new();
    private int requestNumber = 0;
    private string workFolder = "";

    const string SystemPrompt = """
    You are a coding agent that can read, write, and list files.
    All file paths are relative to the working folder — do NOT use absolute paths.
    Use the tools available to you to help the user with their coding tasks.
    Always check existing files before writing new ones.
    """;

    public MySmartAgent(string workFolder)
    {
        Console.WriteLine($"Coding Agent Ready! Working folder: {workFolder}");
        this.workFolder = workFolder;
    }

    public async Task<Message> SendMessage(string userText)
    {
        context.Add(new() { Role = Role.User, Content = userText });

        var response = await CallClaude();
        AddToContext(response);

        return response;
    }


    async Task<Message> CallClaude()
    {
        requestNumber++;
        Console.WriteLine();
        Console.WriteLine($"####################################################################################################################");
        Console.WriteLine($"### Request #{requestNumber}");
        Console.WriteLine($"####################################################################################################################\r\n");

        var table = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]Request Context[/]")
            .ShowRowSeparators()
            .AddColumn("[bold]Entry[/]");

        foreach (var message in context)
        {
            table.AddRow(Markup.Escape(message.ToString()));
        }

        AnsiConsole.Write(table);

        var response = await client.Messages.Create(new MessageCreateParams
        {
            Model = "claude-sonnet-4-5",
            MaxTokens = 4096,
            System = new MessageCreateParamsSystem(SystemPrompt),
            Messages = context,
            Tools = GetToolDefinitions()
        });

        Console.WriteLine();
        Console.WriteLine($"####################################################################################################################");
        Console.WriteLine($"### Response #{requestNumber} (stop_reason: {response.StopReason})");
        Console.WriteLine($"####################################################################################################################");

        var responseTable = new Table()
            .Border(TableBorder.Rounded)
            .Title("[bold]Response Content[/]")
            .ShowRowSeparators()
            .AddColumn("[bold]Type[/]")
            .AddColumn("[bold]Content[/]");

        foreach (var block in response.Content)
        {
            if (block.Value is TextBlock text)
                responseTable.AddRow("text", Markup.Escape(text.Text));
            else if (block.Value is ToolUseBlock toolUse)
                responseTable.AddRow("tool_use", Markup.Escape($"{toolUse.Name}({FormatArgs(toolUse)})"));
        }

        AnsiConsole.Write(responseTable);

        return response;
    }



    public bool WantsToUseTool(Message response)
    {
        return response.StopReason?.Value() == StopReason.ToolUse;
    }

    public async Task<Message> ProcessToolCalls(Message response)
    {
        Console.WriteLine();
        var table = new Table()
                    .Border(TableBorder.Rounded)
                    .Title("[bold]Tool Calls[/]")
                    .ShowRowSeparators()
                    .AddColumn("[bold]Tool[/]")
                    .AddColumn("[bold]Args[/]")
                    .AddColumn("[bold]Result (truncated)[/]");

        // Execute each tool and collect results
        var toolResults = new List<ContentBlockParam>();
        foreach (var block in response.Content)
        {
            if (block.Value is ToolUseBlock toolUse)
            {
                var args = FormatArgs(toolUse);

                var toolResult = ExecuteTool(toolUse);

                table.AddRow(
                    Markup.Escape(toolUse.Name),
                    Markup.Escape(args),
                    Markup.Escape(Truncate(toolResult, 500))
                );

                toolResults.Add(new ContentBlockParam(
                    new ToolResultBlockParam(toolUse.ID) { Content = toolResult }));
            }
        }

        // Only print if we actually had tool calls
        if (table.Rows.Count > 0)
            AnsiConsole.Write(table);

        // Send tool results back to Claude
        context.Add(new() { Role = Role.User, Content = new MessageParamContent(toolResults) });
        var nextResponse = await CallClaude();
        AddToContext(nextResponse);
        return nextResponse;
    }

    string ExecuteTool(ToolUseBlock toolUse)
    {
        switch (toolUse.Name)
        {
            case "read_file":
                var readPath = ResolvePath(toolUse.Input["path"].GetString()!);
                if (readPath is null) return "Error: Path is outside the working folder.";
                return File.Exists(readPath)
                    ? File.ReadAllText(readPath)
                    : $"Error: File not found: {readPath}";

            case "write_file":
                var writePath = ResolvePath(toolUse.Input["path"].GetString()!);
                if (writePath is null) return "Error: Path is outside the working folder.";
                var content = toolUse.Input["content"].GetString()!;
                var dir = Path.GetDirectoryName(writePath);
                if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(writePath, content);
                return $"Successfully wrote to {writePath}";

            case "list_files":
                var listPath = ResolvePath(toolUse.Input["path"].GetString()!);
                if (listPath is null) return "Error: Path is outside the working folder.";
                if (!Directory.Exists(listPath))
                    return $"Error: Directory not found: {listPath}";
                var entries = Directory.GetFileSystemEntries(listPath)
                    .Select(Path.GetFileName);
                return string.Join("\n", entries);

            default:
                return $"Error: Unknown tool: {toolUse.Name}";
        }
    }

    string? ResolvePath(string relative)
    {
        var full = Path.GetFullPath(Path.Combine(workFolder, relative));
        return full.StartsWith(workFolder, StringComparison.OrdinalIgnoreCase) ? full : null;
    }

    List<ToolUnion> GetToolDefinitions()
    {
        List<ToolUnion> tools =
        [
            new Tool
            {
                Name = "read_file",
                Description = "Read the contents of a file at the given path. Use this to examine existing code or files.",
                InputSchema = new InputSchema
                {
                    Properties = new Dictionary<string, JsonElement>
                    {
                        ["path"] = JsonSerializer.SerializeToElement(new { type = "string", description = "The file path to read" })
                    },
                    Required = ["path"]
                }
            },
            new Tool
            {
                Name = "write_file",
                Description = "Write content to a file at the given path. Creates parent directories if they don't exist.",
                InputSchema = new InputSchema
                {
                    Properties = new Dictionary<string, JsonElement>
                    {
                        ["path"] = JsonSerializer.SerializeToElement(new { type = "string", description = "The file path to write to" }),
                        ["content"] = JsonSerializer.SerializeToElement(new { type = "string", description = "The content to write to the file" })
                    },
                    Required = ["path", "content"]
                }
            },
            new Tool
            {
                Name = "list_files",
                Description = "List all files and directories at the given path.",
                InputSchema = new InputSchema
                {
                    Properties = new Dictionary<string, JsonElement>
                    {
                        ["path"] = JsonSerializer.SerializeToElement(new { type = "string", description = "The directory path to list" })
                    },
                    Required = ["path"]
                }
            }
                ];

        return tools;
    }

    public void AddToContext(Message response)
    {
        context.Add(new()
        {
            Role = Role.Assistant,
            Content = new MessageParamContent(
                response.Content.Select(ToContentBlockParam).ToList())
        });
    }

    // --- Low-level Helpers ---

    static ContentBlockParam ToContentBlockParam(ContentBlock block)
    {
        if (block.Value is TextBlock t)
            return new ContentBlockParam(new TextBlockParam { Text = t.Text });
        if (block.Value is ToolUseBlock tu)
            return new ContentBlockParam(new ToolUseBlockParam
            {
                ID = tu.ID,
                Name = tu.Name,
                Input = tu.Input
            });
        throw new InvalidOperationException(
            $"Unexpected content block type: {block.Value?.GetType().Name}");
    }

    static string FormatArgs(ToolUseBlock toolUse)
    {
        var args = toolUse.Input.Select(kv => $"{kv.Key}: {Truncate(kv.Value.ToString(), 60)}");
        return string.Join(", ", args);
    }

    static string Truncate(string value, int maxLength)
    {
        return value.Length <= maxLength ? value : value[..(maxLength - 3)] + "...";
    }
}
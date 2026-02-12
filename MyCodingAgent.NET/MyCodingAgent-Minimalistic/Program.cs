using Anthropic;
using Anthropic.Models.Messages;
using System.Text.Json;


// Sample prompt:
// Create a simple fizz buzz application in C#, with program.cs and project file. Do use .NET 10.

var workFolder = @"c:\MyAgentMini";
Directory.CreateDirectory(workFolder);

var agent = new MySmartAgent(workFolder);

// ========== MINIMALISTIC AGENTIC LOOP ==========

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
    return Console.ReadLine() ?? "";
}

public class MySmartAgent
{
    private AnthropicClient client = new();
    private List<MessageParam> context = new();
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
        var response = await client.Messages.Create(new MessageCreateParams
        {
            Model = "claude-sonnet-4-5",
            MaxTokens = 4096,
            System = new MessageCreateParamsSystem(SystemPrompt),
            Messages = context,
            Tools = GetToolDefinitions()
        });

        // Print response
        foreach (var block in response.Content)
        {
            if (block.Value is TextBlock text)
                Console.WriteLine(text.Text);
        }

        return response;
    }

    public bool WantsToUseTool(Message response)
    {
        return response.StopReason?.Value() == StopReason.ToolUse;
    }

    public async Task<Message> ProcessToolCalls(Message response)
    {
        // Execute each tool and collect results
        var toolResults = new List<ContentBlockParam>();
        foreach (var block in response.Content)
        {
            if (block.Value is ToolUseBlock toolUse)
            {
                var toolResult = ExecuteTool(toolUse);
                toolResults.Add(new ContentBlockParam(
                    new ToolResultBlockParam(toolUse.ID) { Content = toolResult }));
            }
        }

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
        return
        [
            CreateTool("read_file", "Read the contents of a file at the given path. Use this to examine existing code or files.",
                """{"path": {"type": "string", "description": "The file path to read"}}""", ["path"]),
            CreateTool("write_file", "Write content to a file at the given path. Creates parent directories if they don't exist.",
                """{"path": {"type": "string", "description": "The file path to write to"}, "content": {"type": "string", "description": "The content to write to the file"}}""", ["path", "content"]),
            CreateTool("list_files", "List all files and directories at the given path.",
                """{"path": {"type": "string", "description": "The directory path to list"}}""", ["path"])
        ];
    }

    static Tool CreateTool(string name, string description, string propertiesJson, string[] required)
    {
        var properties = JsonDocument.Parse(propertiesJson).RootElement
            .EnumerateObject()
            .ToDictionary(p => p.Name, p => p.Value.Clone());

        return new Tool
        {
            Name = name,
            Description = description,
            InputSchema = new InputSchema { Properties = properties, Required = required }
        };
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
}

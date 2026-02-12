# How Does a Coding Agent Work?

This repository contains demo code for the presentation **"How Does a Coding Agent Work?"** - a hands-on exploration of the core mechanics behind AI coding agents.

## About the Presentation

Coding agents are increasingly used to automate software tasks, yet how they actually work is often treated as a black box. This talk breaks a coding agent down into its core mechanics: how it is structured, how it communicates with a language model, and how a text-only model can read and modify real source code.

**We cover:**
- What a coding agent is, mechanically
- How agents communicate with language models
- How tools and MCP calls are executed
- How a text-only model works with source code
- How to build a basic coding agent in C#

## Repository Structure

This repository contains two implementations of the same coding agent:

### 🎨 MyCodingAgent (Full Version)
The feature-rich version with [Spectre.Console](https://spectreconsole.net/) UI, showing:
- Formatted request/response tables
- Visual tool call tracking
- Rich console output for debugging

### ⚡ MyCodingAgent-Minimalistic
The bare-bones version stripped down to **~210 lines** of code, demonstrating:
- How compact a functional coding agent can be
- Core agentic loop pattern
- Essential tool execution without visual flourishes

**Both versions implement the exact same functionality:**
- Read, write, and list files in a sandboxed folder
- Maintain conversation context with Claude
- Execute tool calls in a loop until completion
- Path security to prevent escaping the working directory

## Quick Start

### Prerequisites
- .NET 10.0 SDK
- Anthropic API key

### Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd MyCodingAgent.NET
   ```

2. **Configure API Key**
   ```bash
   cd MyCodingAgent  # or MyCodingAgent-Minimalistic
   dotnet user-secrets init
   dotnet user-secrets set "Anthropic:ApiKey" "your-api-key-here"
   ```

3. **Run**
   ```bash
   dotnet run
   ```

### Try It Out

Example prompts to test the agent:
```
Create a simple fizz buzz application in C#, with program.cs and project file. Do use .NET 10.

Create a simple calculator class with add, subtract, multiply, and divide methods.

Read the fizzbuzz program and add comments explaining how it works.
```

## How It Works

### The Agentic Loop

At its core, a coding agent is just a simple loop:

```csharp
while (true)
{
    var input = GetUserInput();
    var response = await agent.SendMessage(input);

    // LLM chains tools together (read file -> edit file -> test code)
    while (agent.WantsToUseTool(response))
        response = await agent.ProcessToolCalls(response);
}
```

### The Three Tools

The agent exposes three simple tools to Claude:

1. **read_file** - Read contents of a file
2. **write_file** - Write content to a file (creates directories as needed)
3. **list_files** - List all files and directories in a path

All file operations are sandboxed to a working folder (`./output` by default).

### Message Flow

1. User provides a prompt
2. Agent sends prompt + conversation context + tool definitions to Claude
3. Claude responds with text and/or tool calls
4. Agent executes tools and sends results back
5. Loop continues until Claude has no more tools to call
6. User sees the final response

### Path Security

The `ResolvePath()` method ensures all file paths stay within the working folder:
```csharp
string? ResolvePath(string relative)
{
    var full = Path.GetFullPath(Path.Combine(workFolder, relative));
    return full.StartsWith(workFolder, StringComparison.OrdinalIgnoreCase) ? full : null;
}
```

## Key Concepts Demonstrated

### 1. Tool Use Pattern
Claude's API supports function calling through the "tool use" pattern. The agent:
- Defines tools with JSON schemas
- Claude responds with `ToolUseBlock` objects
- Agent executes tools and returns results as `ToolResultBlockParam`

### 2. Context Management
The agent maintains full conversation history in a `List<MessageParam>`, including:
- User messages
- Assistant responses (text + tool calls)
- Tool results (sent as user messages)

### 3. Stateless API with Stateful Client
Claude's API is stateless - the agent must send the full conversation context on every request. This allows Claude to "remember" previous interactions.

### 4. Sandboxing
The agent restricts all file operations to a designated working folder, preventing Claude from accessing or modifying files outside the sandbox.

## Code Comparison

| Feature | MyCodingAgent | MyCodingAgent-Minimalistic |
|---------|---------------|----------------------------|
| Lines of Code | ~295 | ~210 |
| Dependencies | Anthropic SDK, Spectre.Console | Anthropic SDK only |
| Output | Rich formatted tables | Plain console output |
| Debugging | Request/response tracking | Minimal output |
| Use Case | Development & demos | Understanding core concepts |

## Technical Details

- **Framework**: .NET 10.0
- **Language Model**: Claude Sonnet 4.5
- **API Client**: [Anthropic C# SDK v12](https://github.com/anthropics/anthropic-sdk-dotnet)
- **Max Tokens**: 4096 (to limit costs and prevent runaway token usage)
- **Tool Execution**: Synchronous, sequential

## Further Resources

* <a href="https://nestenius.se/" target="_blank">My personal blog</a>
* <a href="https://tn-data.se/" target="_blank">TN Datakonsult AB</a>, my personal company
* <a href="https://www.linkedin.com/in/torenestenius/" target="_blank">LinkedIn profile</a>, feel free to connect!
* <a href="https://stackoverflow.com/users/68490/tore-nestenius" target="_blank">Stack Overflow profile</a>
* <a href="https://www.meetup.com/net-skane/" target="_blank">.NET Skåne</a> - Local .NET user group

## Additional Resources

- [Anthropic API Documentation](https://docs.anthropic.com/)
- [Claude Tool Use Guide](https://docs.anthropic.com/en/docs/tool-use)
- [Model Context Protocol (MCP)](https://modelcontextprotocol.io/)
- [Spectre.Console Documentation](https://spectreconsole.net/)

## License

This demo code is provided for educational purposes as part of the presentation "How Does a Coding Agent Work?".

---

For more talks and presentations, visit my [Talks page](https://nestenius.se/).

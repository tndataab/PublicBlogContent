# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This repository contains demo code for the presentation **"How Does a Coding Agent Work?"** - demonstrating the core mechanics behind AI coding agents.

Two variants of the same coding agent built with the Anthropic SDK:

- **MyCodingAgent**: Full-featured version (~295 lines) with Spectre.Console UI for rich terminal output - best for demos and development
- **MyCodingAgent-Minimalistic**: Lightweight version (~210 lines) with minimal dependencies - best for understanding core concepts

Both implement an agentic loop where Claude can read, write, and list files in a sandboxed working folder. The goal is to show how simple a functional coding agent can be.

## Build and Run

```bash
# Build and run the full version with UI
cd MyCodingAgent
dotnet run

# Build and run the minimalistic version
cd MyCodingAgent-Minimalistic
dotnet run
```

## Configuration

The `AnthropicClient` default constructor reads the API key from the `ANTHROPIC_API_KEY` environment variable.

**PowerShell (current session only):**
```powershell
$env:ANTHROPIC_API_KEY = "your-api-key-here"
```

**PowerShell (permanent, user-level):**
```powershell
[Environment]::SetEnvironmentVariable("ANTHROPIC_API_KEY", "your-api-key-here", "User")
```

**Command Prompt (current session only):**
```cmd
set ANTHROPIC_API_KEY=your-api-key-here
```

**Command Prompt (permanent, user-level):**
```cmd
setx ANTHROPIC_API_KEY "your-api-key-here"
```
Note: `setx` sets the variable for future sessions. You'll need to open a new terminal for it to take effect.

Run the app from the same terminal where you set the variable.

## Architecture

### Core Components

**Agentic Loop** (Program.cs lines 8-18):
- Gets user input
- Sends message to Claude via `SendMessage()`
- Processes tool calls in a loop until Claude stops requesting tools

**MySmartAgent Class**:
- Maintains conversation context across multiple turns
- Manages API calls to Claude
- Executes three sandboxed tools: `read_file`, `write_file`, `list_files`
- All file operations are restricted to `./output` folder (created automatically)

### Tool Execution Flow

1. Claude requests a tool via `ToolUseBlock` in response
2. `ExecuteTool()` validates paths and performs file operations
3. Results are added to context as `ToolResultBlockParam`
4. Context is sent back to Claude for next turn

### Path Security

The `ResolvePath()` method (line 201-205) ensures all file paths stay within the working folder:
- Takes relative path as input
- Resolves to absolute path within working folder
- Returns `null` if path attempts to escape sandbox

## Key Differences Between Variants

**MyCodingAgent** (Full Version):
- Uses Spectre.Console for formatted tables showing requests, responses, and tool calls
- Rich visual feedback with borders, titles, and markup
- Better for development and debugging

**MyCodingAgent-Minimalistic** (Minimalistic Version):
- Uses only `Console.WriteLine()` for output
- Minimal dependencies (only Anthropic SDK - no Spectre.Console)
- No requestNumber tracking, no truncation helpers, no fancy formatting
- ~210 lines of clean, readable code showing core functionality
- Better for understanding how agents work at a fundamental level

## Model Configuration

The agent uses **Claude Sonnet 4.5** (`claude-sonnet-4-5`).

MaxTokens is set to 4096 to limit costs and prevent runaway token usage. Tool definitions are provided on every API request.

## Context Management

- Full conversation history is maintained in `context` list
- Each user message and assistant response is appended
- Tool results are sent as user messages containing `ToolResultBlockParam` blocks
- Context persists for entire session (no truncation implemented)

## Important Implementation Details

- `ContentBlockParam` conversion: The `ToContentBlockParam()` helper converts API response blocks to request parameter blocks for maintaining context
- Error handling: Returns error strings from tools rather than throwing exceptions
- Working folder: Configurable path passed to MySmartAgent constructor
- Tool definitions: Use `JsonSerializer.SerializeToElement()` to convert anonymous objects to `JsonElement` required by the SDK

## Presentation Context

This code demonstrates how coding agents work mechanically:
1. **The Agentic Loop**: Simple while loop taking user input and processing tool calls
2. **Tool Use Pattern**: How Claude requests tools via `ToolUseBlock` and receives results via `ToolResultBlockParam`
3. **Context Management**: How conversation history is maintained across stateless API calls
4. **Sandboxing**: How `ResolvePath()` restricts file operations to working folder
5. **Simplicity**: A functional coding agent in ~210 lines (minimalistic) or ~295 lines (with UI)

## Common Extensions for Live Demos

When extending the agent during the presentation, consider adding:

1. **New Tools**:
   - `run_command` - Execute shell commands (with safety checks)
   - `search_files` - Search for text across multiple files
   - `delete_file` - Remove files

2. **Better Context Management**:
   - Truncate old messages when context gets too long
   - Save conversation history to disk

3. **Streaming Responses**:
   - Use `client.Messages.CreateStream()` for real-time output
   - Show text tokens as they arrive

4. **MCP Integration**:
   - Connect to Model Context Protocol servers
   - Add filesystem, git, or database tools via MCP

## Testing

Try these prompts to demonstrate agent capabilities:
- "Create a simple fizz buzz application in C#, with program.cs and project file. Do use .NET 10."
- "Create a calculator class with add, subtract, multiply, and divide methods."
- "Read the fizzbuzz program and add comments explaining how it works."
- "List all files in the current directory and tell me what they contain."

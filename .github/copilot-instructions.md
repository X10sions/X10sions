# GitHub Copilot Custom Instructions

## Git Commit Message Standards
When generating commit messages, follow these specific formatting rules:

- **Style**: Use the [Conventional Commits](https://www.conventionalcommits.org) standard (e.g., `feat:`, `fix:`, `chore:`).
- **Dependency Updates**: If the changes involve updating dependencies (e.g., modifications to `.csproj`, `package.json`, or lock files), you MUST include a bulleted list in the message body with the following format:
  * [Package Name]: [Old Version] -> [New Version]
- **Mood**: Use the imperative mood in the subject line (e.g., "Add" instead of "Added").
- **Length**: Keep the subject line under 50 characters.

## Project Context
- **Primary Framework**: .NET 8 / C# 12
- **Testing**: We use xUnit for unit tests. Always suggest xUnit patterns for new test files.

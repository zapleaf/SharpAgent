# SharpAgent

SharpAgent is a C# AI Agent application that uses WebAPI endpoints to trigger intelligent agent workflows. Built with .NET 8, Entity Framework, and SQL Server, it provides a framework for creating automated AI processes through composable services.

## Table of Contents
- [Overview](#overview)
- [Architecture](#architecture)
  - [Workflow Structure](#workflow-structure)
  - [Core Components](#core-components)
  - [Workflow Execution](#workflow-execution)
- [Key Features](#key-features)
- [Technical Stack](#technical-stack)
- [Getting Started](#getting-started)

## Overview

SharpAgent is designed as a framework for creating AI-powered automated processes rather than an application for creating agents. It leverages various services including vector databases, document processing, and AI services to create flexible workflows. These workflows can be purely computational or involve AI agents, all triggered through API endpoints.

## Architecture

SharpAgent implements Clean Architecture principles, separating concerns into distinct layers:

- **API Layer**: WebAPI endpoints for triggering workflows
- **Blazor Layer**: Blazor layer currently only has YouTube video listings
- **Application Layer**: Business logic and workflow orchestration
- **Domain Layer**: Core business entities and interfaces
- **Infrastructure Layer**: External service implementations and data access

### Workflow Structure

SharpAgent uses a simplified approach where each process has its own Handler containing the necessary conditions, decisions, and business logic. This direct implementation approach was chosen over a more abstract JSON-based workflow system to maximize development efficiency.

### Core Components

1. **Handlers**
   - Currently there is no structure that needs to be followed
   - We are considering the following:
     - Orchestrate multiple agents working together
     - Track execution state and history
     - Support parallel agent collaboration

2. **Agents**
   - Currently we just support prompt versioning 
   - Would like to create full agents that:
     - Represent specialized AI personas
     - Define roles and responsibilities

3. **Current Services**
   - Document Analysis (Azure Document Intelligence)
   - OpenAI Chat Integration
   - Embeddings Generation (OpenAI)
   - Vector Storage (Pinecone)
   - YouTube API
   - ApiFy Integration *planned next*

### Workflow Execution

- Each workflow is defined in a Handler
- Client triggers workflow via POST to specific endpoint
- Parameters are provided to endpoint

## Key Features

- **Service Discovery**: Automatic discovery of available commands through reflection
- **State Management**: Complete workflow state tracking and persistence
- **Error Handling**: Comprehensive error capture and logging
- **Extensibility**: Easy addition of new services and agent types
- **Data Flow**: Seamless data passing between workflow steps

## Technical Stack

- **.NET 8**
- **Entity Framework Core**
- **SQL Server**
- **MediatR** for CQRS pattern
- **AutoMapper** for object mapping
- **FluentValidation** for request validation

## Getting Started

1. Clone the repository
2. Configure external services in `appsettings.json` with your API keys and connection strings
3. Run the database migrations: `dotnet ef database update`
4. Start the application: `dotnet run`

Refer to the documentation for detailed setup instructions for specific services like Pinecone, OpenAI, etc.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

[MIT](LICENSE)
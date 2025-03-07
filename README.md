# SharpAgent

SharpAgent is a C# AI Agent application that uses WebAPI endpoints to trigger intelligent agent workflows. Built with .NET 8, Entity Framework, and SQL Server, it provides a framework for creating automated AI processes through composable services.

## Overview

SharpAgent is designed as a framework for creating AI-powered automated processes rather than an application for creating agents. It leverages various services including vector databases, document processing, and AI services to create flexible workflows. These workflows can be purely computational or involve AI agents, all triggered through API endpoints.

## Architecture

SharpAgent implements Clean Architecture principles, separating concerns into distinct layers:

- **API Layer**: WebAPI endpoints for triggering workflows
- **Application Layer**: Business logic and workflow orchestration
- **Domain Layer**: Core business entities and interfaces
- **Infrastructure Layer**: External service implementations and data access

### Workflow Structure

Originally I setup workflow system where each process would have a workflow id and the various steps would be outlined in JSON. However, after the first version it felt I would spend more time creating a flexible "workflow" that creating actual processes.

So this was scrapped and now each process will have its own Handler which will contain any conditions, decisions, and business logic.

### Core Components

1. **Handlers**
   - Orchestrate multiple agents working together
   - Define sequence of steps using different services
   - Track execution state and history
   - Support parallel agent collaboration

2. **Agents**
   - Represent specialized AI personas
   - Define roles and responsibilities
   - Maintain consistent behavior through configurations
   - Support versioning for refinement

3. **Services**
   Built-in services include:
   - Document Analysis (Azure Document Intelligence)
   - OpenAI Chat Integration
   - Embeddings Generation (OpenAI)
   - Vector Storage (Pinecone)
   - YouTube API *planned next*
   - ApiFy Integration *planned next*

### Workflow Execution

1. **Initialization**
   - Client triggers workflow via POST to specific endpoint
   - Optional parameters can be provided

2. **Step Processing**
   - Each step associates with:
     - Specific service type (e.g., "Embedding", "VectorDb")
     - Agent role (e.g., "Researcher", "Analyst")
     - Configuration parameters
   - Results flow between steps
   - Progress is tracked in database

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

Be sure to setup any external services you wish to use and include the api key / connection strings in the appsettings.json file. I will have to expand this more later.





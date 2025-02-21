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

The workflow system is built around several key entities:

1. **AgentWorkflow**
   - Contains sequence of steps (stored as JSON in `WorkflowSteps`)
   - Tracks participating `Agent` entities
   - Maintains collection of `AgentTask` records
   - Monitors status, current step, and execution times

2. **WorkflowHandler**
   - Core orchestrator for workflow execution
   - Discovers available commands via reflection
   - Maps steps to appropriate services and agents
   - Manages data flow between steps

3. **WorkflowRepository**
   - Manages workflow state in database
   - Tracks execution progress
   - Records step results
   - Handles workflow completion

4. **Service Commands**
   - Tagged with `ServiceCommandAttribute`
   - Mapped to specific service types
   - Automatically discovered and registered
   - Handle individual step execution

## Agent Workflow System

### Core Components

1. **Workflows**
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
   - Client triggers workflow via POST `/api/workflow/{workflowId}`
   - Optional parameters can be provided
   - System loads workflow definition and participating agents

2. **Step Processing**
   - Each step associates with:
     - Specific service type (e.g., "Embedding", "VectorDb")
     - Agent role (e.g., "Researcher", "Analyst")
     - Configuration parameters
   - Results flow between steps
   - Progress is tracked in database

3. **Example Workflow Structure**
```json
{
  "steps": [
    {
      "order": 1,
      "serviceType": "Embedding",
      "agentRole": "Researcher",
      "parameters": {}
    },
    {
      "order": 2,
      "serviceType": "VectorDb",
      "agentRole": "Analyst",
      "parameters": {}
    }
  ]
}
```

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





# Task Management System

## Descrição do Relacionamento entre Entidades

Este sistema de gerenciamento de tarefas foi projetado seguindo os princípios de Domain-Driven Design (DDD). Abaixo está uma descrição do relacionamento entre as principais entidades do sistema:

### Entidades

#### 1. **User**
- Representa o usuário do sistema.
- Propriedades principais:
  - `Id`: Identificador único do usuário.
  - `Name`: Nome do usuário.
  - `Email`: Endereço de e-mail do usuário.

#### 2. **Project**
- Representa um projeto que contém várias tarefas.
- Propriedades principais:
  - `Id`: Identificador único do projeto.
  - `Name`: Nome do projeto.
  - `UserId`: Identificador do usuário que criou o projeto.
  - `Tasks`: Lista de tarefas associadas ao projeto.
  - `MaxTasks`: Constante que define o limite máximo de 20 tarefas por projeto.

#### 3. **Task**
- Representa uma unidade de trabalho dentro de um projeto.
- Propriedades principais:
  - `Id`: Identificador único da tarefa.
  - `Title`: Título da tarefa.
  - `Description`: Descrição detalhada da tarefa.
  - `DueDate`: Data de vencimento da tarefa.
  - `Status`: Status atual da tarefa (e.g., Pendente, Em Progresso, Concluída).
  - `Priority`: Prioridade da tarefa (e.g., Baixa, Média, Alta).
  - `History`: Lista de alterações realizadas na tarefa, incluindo informações sobre o que foi modificado, a data da modificação e o usuário responsável.

### Relacionamentos

1. **User e Project**
   - Um usuário pode criar e gerenciar vários projetos.
   - Cada projeto pertence a um único usuário, identificado pela propriedade `UserId` na entidade `Project`.

2. **Project e Task**
   - Um projeto pode conter várias tarefas, representadas pela lista `Tasks` na entidade `Project`.
   - Cada tarefa pertence a um único projeto.
   - O número máximo de tarefas por projeto é limitado a 20, conforme definido pela constante `MaxTasks` na entidade `Project`.

3. **Task e History**
   - Cada tarefa possui um histórico de alterações, representado pela lista `History` na entidade `Task`.
   - O histórico registra informações sobre modificações realizadas na tarefa, incluindo o que foi alterado, a data da alteração e o usuário responsável.

## Estrutura do Projeto

O projeto está organizado em camadas seguindo o padrão DDD:
- **Domain**: Contém as entidades principais (`User`, `Project`, `Task`) e as regras de negócio.
- **Application**: Contém os casos de uso e serviços de aplicação.
- **Infrastructure**: Contém a implementação de repositórios e acesso a banco de dados.
- **API**: Contém os controladores e endpoints da API.

## Regras de Negócio

1. **Prioridades de Tarefas**:
   - Cada tarefa deve ter uma prioridade atribuída (Baixa, Média, Alta).
   - A prioridade não pode ser alterada após a criação da tarefa.

2. **Restrições de Remoção de Projetos**:
   - Um projeto não pode ser removido se houver tarefas pendentes associadas a ele.

3. **Histórico de Atualizações**:
   - Cada alteração em uma tarefa deve ser registrada no histórico de alterações.

4. **Limite de Tarefas por Projeto**:
   - Cada projeto pode conter no máximo 20 tarefas.

5. **Comentários nas Tarefas**:
   - Os usuários podem adicionar comentários às tarefas, que serão registrados no histórico de alterações.

## Observações

Este README será atualizado conforme o projeto evoluir e novas funcionalidades forem implementadas.
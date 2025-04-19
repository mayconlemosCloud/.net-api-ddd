# .NET API DDD Project

## Pré-requisitos

Certifique-se de que você tenha os seguintes softwares instalados em sua máquina:

- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0)

## Como rodar o projeto

### 1. Clonar o repositório

Clone este repositório em sua máquina local:
```bash
git clone <URL_DO_REPOSITORIO>
cd .net-api-ddd
```

### 2. Configurar o ambiente

Certifique-se de que o arquivo `appsettings.Development.json` está configurado corretamente para o ambiente de desenvolvimento. Verifique as configurações de conexão com o banco de dados e outros parâmetros necessários.

### 3. Construir e rodar os containers Docker

Execute o seguinte comando para construir e iniciar os containers Docker:
```bash
docker-compose up --build
```

Este comando irá:
- Construir a imagem Docker para a API usando o `Dockerfile`.
- Iniciar o serviço da API na porta `8080`.
- Iniciar o serviço PostgreSQL na porta `5432`.

### 4. Acessar a API

Após os containers estarem em execução, você pode acessar a API em:
```
http://localhost:8080
```



## Estrutura do Projeto

- **API/**: Contém os controladores e configurações da API.
- **Application/**: Contém os serviços, DTOs, mapeamentos e validações.
- **Domain/**: Contém as entidades e interfaces de repositório.
- **Infrastructure/**: Contém o contexto do banco de dados, migrações e implementações de repositório.
- **IoC/**: Configuração de injeção de dependência.

## Comandos Úteis

- Parar os containers:
```bash
docker-compose down
```

- Reconstruir os containers sem cache:
```bash
docker-compose build --no-cache
```

- Verificar logs dos containers:
```bash
docker-compose logs -f
```

## Contribuição

Sinta-se à vontade para contribuir com este projeto. Faça um fork, crie um branch e envie um pull request.

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

# Fase 2: Refinamento

Perguntas para futuras melhorias:

1. Quais funcionalidades podem ser adicionadas?
2. Há necessidade de integração com outros sistemas?
3. Quais métricas devem ser monitoradas?
4. Planejamos implementar autenticação no futuro?
5. Algum padrão arquitetural deve ser adotado?
6. Como melhorar a interface da API?
7. Há necessidade de suporte a múltiplos idiomas?

# Fase 3: Final

Melhorias sugeridas para o projeto:

1. **Pontos de Melhoria**:
   - Melhorar a cobertura de testes unitários para atingir 100%.
   - Implementar logs estruturados para facilitar a depuração e monitoramento.

2. **Padrões de Implementação**:
   - Adotar o padrão CQRS (Command Query Responsibility Segregation) para separar operações de leitura e escrita.
   - Utilizar o padrão de repositório com Unit of Work para gerenciar transações.

3. **Visão sobre Arquitetura/Cloud**:
   - Migrar a aplicação para uma arquitetura baseada em microsserviços, se necessário, para maior escalabilidade.
   - Configurar pipelines de CI/CD para automação de testes e deploy.
   - Utilizar serviços gerenciados na nuvem, como AWS RDS ou Azure SQL, para o banco de dados.

Essas melhorias visam aumentar a qualidade, escalabilidade e manutenção do projeto.
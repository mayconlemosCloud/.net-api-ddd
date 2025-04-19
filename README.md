# Sistema de Gerenciamento de Tarefas (.NET API DDD)

## Visão Geral

O objetivo deste projeto é criar uma API RESTful para gerenciamento de tarefas, permitindo que usuários organizem e monitorem suas tarefas diárias, além de colaborar com colegas de equipe.

## Funcionalidades

- **Listagem de Projetos:** Listar todos os projetos do usuário.
- **Visualização de Tarefas:** Visualizar todas as tarefas de um projeto específico.
- **Criação de Projetos:** Criar um novo projeto.
- **Criação de Tarefas:** Adicionar uma nova tarefa a um projeto.
- **Atualização de Tarefas:** Atualizar o status ou detalhes de uma tarefa.
- **Remoção de Tarefas:** Remover uma tarefa de um projeto.

## Regras de Negócio

1. **Prioridades de Tarefas:**
   - Cada tarefa deve ter prioridade (baixa, média, alta).
   - Não é permitido alterar a prioridade após a criação.

2. **Restrições de Remoção de Projetos:**
   - Projetos com tarefas pendentes não podem ser removidos.
   - A API retorna erro e sugere conclusão ou remoção das tarefas pendentes.

3. **Histórico de Atualizações:**
   - Toda atualização em tarefa registra histórico (o que foi modificado, data, usuário).

4. **Limite de Tarefas por Projeto:**
   - Máximo de 20 tarefas por projeto.

5. **Relatórios de Desempenho:**
   - Endpoints para relatórios, como número médio de tarefas concluídas por usuário nos últimos 30 dias.
   - Apenas usuários com função "gerente" podem acessar.

6. **Comentários nas Tarefas:**
   - Usuários podem adicionar comentários.
   - Comentários são registrados no histórico de alterações.

## Regras da API e Avaliação


- Não há autenticação (será serviço externo).
- Cobertura de testes unitários mínima de 80% para regras de negócio.
- Uso de git para versionamento.
- Persistência em banco de dados (livre escolha).
- Frameworks e libs à escolha do desenvolvedor.
- Projeto deve rodar em Docker.
- Instruções de execução via terminal neste README.

---

## Instruções de Execução via Docker Compose

1. **Suba os containers:**
   ```sh
   docker-compose up -d
   ```

2. **Acesso à API:**
   - Acesse via `http://localhost:8080`

3. **Para parar os containers:**
   ```sh
   docker-compose down
   ```

---

## Como executar os testes unitários

1. **Execute os testes unitários com o comando:**
   ```sh
   dotnet test
   ```
2. **Verifique o relatório de cobertura de testes:**
   - Após rodar os testes, um relatório de cobertura será gerado (exemplo: `coverage-report`).
   - Analise os arquivos e métodos não cobertos para criar novos testes e aumentar a cobertura.

---

# Fase 2: Refinamento

Perguntas para futuras melhorias:

1. Quais funcionalidades podem ser adicionadas?
2. Há necessidade de integração com outros sistemas?
3. Quais métricas devem ser monitoradas?
4. Planejamos implementar autenticação no futuro?
5. Algum padrão arquitetural deve ser adotado?
6. Como melhorar a interface da API?
7. Há necessidade de suporte a múltiplos idiomas?

---

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

---

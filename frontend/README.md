# Rotas da API

## Regras de Negócio

- **Prioridade de Tarefas:** Cada tarefa deve ter prioridade (baixa, média, alta) e não pode ser alterada após a criação.
- **Remoção de Projetos:** Não é permitido remover projetos com tarefas pendentes. A API retorna erro e sugere concluir ou remover as tarefas primeiro.
- **Histórico de Atualizações:** Toda atualização em tarefa registra histórico do que foi modificado, data e usuário.
- **Limite de Tarefas por Projeto:** Máximo de 20 tarefas por projeto.
- **Relatórios:** Endpoint para relatório de performance de usuário (média de tarefas concluídas nos últimos 30 dias).
- **Comentários:** Usuários podem adicionar comentários em tarefas, registrados no histórico.
- **Validações:** Nome e e-mail do usuário obrigatórios e válidos; nome do projeto obrigatório e até 100 caracteres; título da tarefa obrigatório e até 100 caracteres; descrição até 500 caracteres; datas válidas.

## Usuários (`/api/User`)
- `GET /api/User` — Listar usuários
- `GET /api/User/{id}` — Obter usuário por ID
- `POST /api/User` — Criar usuário
- `PUT /api/User/{id}` — Atualizar usuário
- `DELETE /api/User/{id}` — Remover usuário

## Projetos (`/api/Project`)
- `GET /api/Project` — Listar projetos
- `GET /api/Project/{id}` — Obter projeto por ID
- `POST /api/Project` — Criar projeto
- `PUT /api/Project/{id}` — Atualizar projeto
- `DELETE /api/Project/{id}` — Remover projeto

## Tarefas (`/api/Task`)
- `GET /api/Task` — Listar tarefas
- `GET /api/Task/{id}` — Obter tarefa por ID
- `POST /api/Task` — Criar tarefa
- `PUT /api/Task/{id}` — Atualizar tarefa
- `DELETE /api/Task/{id}` — Remover tarefa

## Relatórios (`/api/Report`)
- `GET /api/Report/UserPerformance` — Relatório de performance de usuário

> Observação: Os endpoints podem variar conforme implementação. Consulte os controllers para detalhes de parâmetros e payloads.

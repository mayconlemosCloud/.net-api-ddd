# Changelog

## [Unreleased]

### Added
- [x] Estrutura inicial do projeto seguindo o padrão DDD.
- [x] Entidades principais: `User`, `Project`, `TaskEntity`, `TaskHistory`.
- [x] Configuração do `DbContext` e mapeamento das entidades no método `OnModelCreating`.
- [x] Validações para tarefas com `TaskValidator`.
- [x] Configuração de injeção de dependência na camada `Infrastructure`.
- [x] Configuração inicial do banco de dados com suporte ao PostgreSQL.
- [x] Arquivo `docker-compose.yml` para configuração de ambiente.
- [x] Documentação inicial no arquivo `README.md`.
- [x] Funcionalidade de Comentários.
- [x] Funcionalidade de Relatórios.
- [ ] Listagem de Projetos:
  - [ ] Criar Service para listagem de projetos.
  - [x] Criar Repository para listagem de projetos.
  - [ ] Criar Controller para listagem de projetos.
- [ ] Visualização de Tarefas:
  - [ ] Criar Service para visualização de tarefas.
  - [x] Criar Repository para visualização de tarefas.
  - [ ] Criar Controller para visualização de tarefas.
- [ ] Criação de Projetos:
  - [ ] Criar Service para criação de projetos.
  - [x] Criar Repository para criação de projetos.
  - [ ] Criar Controller para criação de projetos.
- [ ] Criação de Tarefas:
  - [ ] Criar Service para criação de tarefas.
  - [x] Criar Repository para criação de tarefas.
  - [ ] Criar Controller para criação de tarefas.
- [ ] Atualização de Tarefas:
  - [ ] Criar Service para atualização de tarefas.
  - [x] Criar Repository para atualização de tarefas.
  - [ ] Criar Controller para atualização de tarefas.
- [ ] Remoção de Tarefas:
  - [ ] Criar Service para remoção de tarefas.
  - [x] Criar Repository para remoção de tarefas.
  - [ ] Criar Controller para remoção de tarefas.

### Changed
- [x] N/A
- [x] Melhorias nas validações de Tarefas e Projetos.
- [x] Atualizações no contexto do banco de dados (`TaskManagementDbContext`).

### Fixed
- [x] N/A

## [0.1.0] - 2025-04-14

### Added
- [x] Commit inicial com toda a estrutura do projeto.
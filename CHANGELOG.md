# Changelog

## [Unreleased]

### Added
- Estrutura inicial do projeto seguindo o padrão DDD.
- Entidades principais: `User`, `Project`, `TaskEntity`, `TaskHistory`.
- Configuração do `DbContext` e mapeamento das entidades no método `OnModelCreating`.
- Validações para tarefas com `TaskValidator`.
- Configuração de injeção de dependência na camada `Infrastructure`.
- Configuração inicial do banco de dados com suporte ao PostgreSQL.
- Arquivo `docker-compose.yml` para configuração de ambiente.
- Documentação inicial no arquivo `README.md`.

### Changed
- N/A

### Fixed
- N/A

## [0.1.0] - 2025-04-14

### Added
- Commit inicial com toda a estrutura do projeto.
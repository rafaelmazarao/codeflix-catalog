# CodeFlix.Catalog

Projeto de estudo de **Clean Architecture** e **Domain-Driven Design (DDD)** em .NET, construído como parte do meu aprendizado sobre um catálogo de vídeos estilo streaming (o famoso "admin catálogo" usado em cursos de arquitetura .NET).

## Objetivo

Praticar:
- Modelagem de domínio rico (entidades que se autovalidam, sem anemic domain model)
- Separação em camadas (Domain / Application / Infra / API)
- Casos de uso isolados (padrão Input/Output)
- Testes unitários com xUnit, FluentAssertions, Bogus e Moq
- Inversão de dependência entre camadas

## Estrutura

```
src/
  Codeflix.Catalog.Domain/        # Entidades, regras de negócio, interfaces de repositório
  Codeflix.Catalog.Application/   # Casos de uso (use cases)
tests/
  Codeflix.Catalog.UnitTests/     # Testes unitários de Domain e Application
```

## Status

🚧 Em desenvolvimento — funcionalidades sendo adicionadas conforme avanço nos estudos.

Até o momento:
- [x] Entidade `Category` com validações de domínio
- [x] Caso de uso `CreateCategory`
- [ ] Demais casos de uso de Category (Update, Delete, Get, List)
- [ ] Camada de infraestrutura (persistência)
- [ ] API

## Como rodar os testes

```bash
dotnet test
```

## Stack

- .NET 6
- xUnit / FluentAssertions / Moq / Bogus

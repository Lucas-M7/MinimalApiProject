# Descrição do Projeto

Este projeto é uma criação de uma API utilizando a técnica de Minimal APIs
para o registro de veículos, ampliando suas funcionalidades ao incorporar administradores com regras de Token JWT que são válidos por 1 dia, para Inserir, Atualizar, Editar e Listar os veículos.

Ao fazer o Login, temos dois tipos de perfis: ADM e Editor
O ADM tem permissão para todas as funcionalidades do projeto.
O Editor é privado a algumas funcionalidades, como criar, atualizar e deletar os veículos.
Neste projeto o usuário poderá listar e registrar os veículos registrado com ambos os perfis.

Caso for um veículo/perfil que não exista, irá retornar um 404.
Caso o usuário tiver um perfil de editor e queira deletar/atualizar/listar perfis, irá retornar um 403.

Os perfis e veículos são mantidos no banco de dados - SQL Server.

## Stack Utilizada

C# - .NET - Entity Framework - MSTest
SQL Server

## Detalhes

 - CRUD.
 - Criação de uma Minimal API.
 - Injeção de Dependência.
 - Login de Usuários.
 - Programação Defensiva.
 - Segurança com Token JWT.
 - Utilização de Banco de Dados.
 - Validação de Usuários.
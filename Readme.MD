# Minimal API Project

Este projeto, como já diz no título, é uma "Minimal API" que é um conceito que se originou no contexto de desenvolvimento de software, especialmente em frameworks e plataformas de desenvolvimento web. Em termos gerais, refere-se a abordagens que buscam simplificar e reduzir a complexidade na criação de interfaces de programação de aplicativos (APIs), ou seja, uma API Mínima, mantendo apenas o essencial necessário para alcançar um objetivo específico.

Essa abordagem é frequentemente associada a frameworks e tecnologias que oferecem uma sintaxe mais limpa e minimalista para definir rotas, controladores e lógica de aplicativo, eliminando ou minimizando a quantidade de código boilerplate ou configuração necessária.

No contexto específico do desenvolvimento web, as "Minimal APIs" podem se referir a frameworks que permitem criar rapidamente APIs simples e eficazes com menos código, como por exemplo o ASP.NET Core da Microsoft ( o meu caso ) ou o Express.js para Node.js. Esses frameworks frequentemente oferecem funcionalidades que permitem aos desenvolvedores criar APIs com menos configuração e código, focando na produtividade e simplicidade do desenvolvimento.

## Imagem

![Imagem do Projeto - API(src/imagem-api.jpg)


## Descrição do Projeto

Eu o criei com foco no treino e também para montar um bom portfólio.

Essa API foi criado na temática de veículos e cadastros e logins de usuários, vou explicar melhor.

Nela, temos uma operação POST logo de início, ou seja, vai inserir/criar algo no banco de dados, certo? Mas nesse caso tem um "/login" no final, então o usuário vai fazer o Login com base nas contas registradas no banco de dados e desse login vai ser gerado um TOKEN JWT válido por 1 dia.

Diante disso, temos dois tipos de perfis:
 - ADM
 - Editor

ADM => Ele tem acesso a todas as funcionalidades da API, desde a criação de um usuário até a remoção de um veículo.

Editor => Ele é restrito a algumas funcionalidades, como: Criar um usuário, listar usuários, adicionar ou atualizar ou remover veículos.

O segundo método POST: "/administradores" é usado para criar um perfil, mas depois de logado em um já existente.

O primeiro método GET: "/administradores" é usado como uma página, que por padrão já vem na primeira página para listar algumas informações dos usuários existentes, como: Tipo de perfil e Email cadastrado.

O segundo método GET: "/administradores/{id}" é usado para buscar usuários pelo ID, que é uma Chave Primária, ou seja, única.

Na parte de Veículos temos um CRUD:

 - POST => Vai registrar um veículo.
 - 2x GET => O 1° vai listar todos os veículos registrados e o 2° vai buscar pelo ID.
 - PUT => Vai atualizar informações de um veículo pelo ID.
 - DELETE => Vai deletar um veículo pelo ID.

 ## Detalhes

 Para mim esse foi o primeiro projeto Backend significativo, pois teve vários conhecimentos em prática e também uma documentação que acredito que seja boa, para uma primeira vez documentando um projeto.

Tecnologias utilizadas:
 - C#
 - Sql Server
 - API ASP NET Core
 - Entity Framework global e suas bibliotecas: EF.Design, EF.SqlServe e EF.Tools
 - Biblioteca para gerar o Token JWT - Microsoft.AspNetCore.Authentication.JwtBearer
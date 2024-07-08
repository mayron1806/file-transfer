# .NET 8 API Template

## Descrição

Este template de API foi desenvolvido em .NET 8 e inclui um sistema de autenticação básico. Ele possui os seguintes endpoints:

1. **Criar Conta**: Permite criar uma nova conta de usuário.
2. **Ativar Conta**: Ativa a conta de usuário.
3. **Login**: Realiza a autenticação do usuário.
4. **Iniciar Esquecimento de Senha**: Envia um e-mail para redefinir a senha.
5. **Redefinir Senha**: Permite redefinir a senha do usuário.

A API utiliza SQLite como banco de dados e possui a documentação configurada com Swagger.

## Configuração

Para rodar o projeto, siga os passos abaixo:

1. Faça o download do template e extraia-o na pasta desejada.
2. Dentro da pasta `API`, existe um arquivo `appsettings.json`. Renomeie ou duplique este arquivo conforme o ambiente que deseja rodar. Por exemplo, para o ambiente de desenvolvimento, renomeie para `appsettings.Development.json`.
3. Preencha as variáveis necessárias no arquivo `appsettings.[Environment].json`.

## Rodando o Projeto

Para rodar o projeto, você deve estar na raiz do projeto. Utilize os comandos abaixo conforme necessário:

### Executar o Projeto

```sh
dotnet run --project ./API/API.csproj --environment "Development"
```

### Executar o Projeto com Watch

```sh
dotnet watch run --project ./API/API.csproj --environment "Development"
```

O valor de `--environment` define o ambiente que está rodando.

## Migrations

### Criar Migration

```sh
dotnet ef migrations add MIGRATION_NAME --project ./Infrastructure/Infrastructure.csproj -s ./API/API.csproj
```

### Aplicar Migration

```sh
dotnet ef database update -s ./API/API.csproj
```

### Remover Migration

```sh
dotnet ef migrations remove --project ./Infrastructure/Infrastructure.csproj -s ./API/API.csproj
```

### Listar Migrations

```sh
dotnet ef migrations list --project ./Infrastructure/Infrastructure.csproj -s ./API/API.csproj
```

## Swagger

A documentação da API pode ser acessada através do Swagger na seguinte URL:

[http://localhost:5000/swagger/](http://localhost:5000/swagger/)

## Contribuição

Sinta-se à vontade para contribuir com este template enviando pull requests ou relatando problemas no repositório.
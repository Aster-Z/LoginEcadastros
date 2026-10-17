# DoaHub — Login e áreas por perfil (Doador / Instituição)

Projeto ASP.NET Core MVC (.NET 8) com ASP.NET Core Identity, pronto para abrir
no Visual Studio. Já tem: cadastro, login, dois perfis (`Doador` e
`InstituicaoColetora`), e painéis protegidos por perfil.

## 1. Pré-requisitos

- Visual Studio 2022 (17.8+) com a carga de trabalho **"Desenvolvimento ASP.NET
  e web"** instalada.
- SQL Server LocalDB (já vem com o Visual Studio, na opção "Armazenamento e
  processamento de dados" do instalador).

## 2. Abrir o projeto

1. Extraia o zip.
2. Dê duplo clique em `DoaHub.sln` para abrir no Visual Studio.
3. Deixe o Visual Studio restaurar os pacotes NuGet automaticamente (ou
   clique com o botão direito na solução → **Restaurar Pacotes NuGet**).

## 3. Criar o banco de dados

O projeto usa Entity Framework Core com Code First. No **Console do Gerenciador
de Pacotes** (Ferramentas → Gerenciador de Pacotes NuGet → Console do
Gerenciador de Pacotes), com o projeto `DoaHub` selecionado como "Default
project":

```powershell
Add-Migration InitialCreate
Update-Database
```

Isso cria o banco `DoaHubDb` no LocalDB com as tabelas do Identity
(usuários, roles, etc.). Se preferir o terminal:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

> Se você já tinha criado o banco numa versão anterior deste projeto (antes
> dos campos de telefone, tipo sanguíneo, CNPJ etc. existirem em
> `ApplicationUser`), rode mais uma migration em cima da que já existe:
> `Add-Migration AddCamposPerfil` seguido de `Update-Database` — não precisa
> apagar o banco nem repetir o `InitialCreate`.

## 4. Rodar

Aperte **F5** (ou Ctrl+F5 para rodar sem debugar). A página inicial (`/`) já
cai direto na tela de **login** (`/entrar`). De lá:

- **Sou doador** → `/cadastro/doador` (nome, e-mail, telefone, tipo
  sanguíneo, cidade/estado, senha).
- **Sou instituição** → `/cadastro/instituicao` (nome, CNPJ, tipo, endereço,
  contato, responsável, senha).

Cada cadastro já loga automaticamente e manda para o painel certo. Errar a
senha no login mostra a mensagem de erro em vermelho acima do formulário.

Teste o fluxo:

1. Na tela de login, clique em **Sou doador** e cadastre um usuário.
2. Você é logado automaticamente e redirecionado para `/Doador` (o painel do
   doador).
3. Clique em **Sair**, volte ao login e clique em **Sou instituição** para
   criar uma segunda conta — ela cai em `/Instituicao`.
4. Tente acessar `/Instituicao` logado como doador: o `[Authorize(Roles = ...)]`
   bloqueia e manda para a tela de acesso negado.
5. No login, digite uma senha errada para ver a mensagem de erro em vermelho.

## 5. Como o controle de perfil funciona

- `Models/Roles.cs` guarda os nomes dos dois perfis como constantes, para não
  depender de strings soltas.
- Os dois cadastros são ações separadas em `AccountController`:
  `RegistrarDoador` (rota `/cadastro/doador`) e `RegistrarInstituicao` (rota
  `/cadastro/instituicao`). Cada uma salva o usuário já com
  `UserManager.AddToRoleAsync(usuario, Roles.Doador)` ou `Roles.Instituicao`.
- Os campos de cada formulário (telefone, tipo sanguíneo, cidade/estado para
  doador; CNPJ, tipo, endereço, responsável para instituição) ficam no
  próprio `ApplicationUser` — simples para esta fase do projeto, mas pode
  virar tabelas de perfil separadas mais pra frente.
- Os painéis ficam em controllers separados, cada um com
  `[Authorize(Roles = Roles.Doador)]` ou `[Authorize(Roles = Roles.Instituicao)]`.
  Qualquer Action dentro desses controllers já herda essa proteção.
- `HomeController.Index` verifica `User.IsInRole(...)` e redireciona quem já
  está logado direto para o painel certo; quem não está logado cai no login
  (`/entrar`), que também é a página inicial do site.

Para adicionar uma nova tela exclusiva de um perfil: crie a Action dentro do
`DoadorController` ou `InstituicaoController` (ela já nasce protegida), ou
crie um novo controller e repita o atributo `[Authorize(Roles = ...)]`.

## 6. Próximos passos (fora do escopo deste pacote)

- Modelos de domínio: `Campanha`, `Interesse`, `Instituicao` (dados extras
  como CNPJ), etc. — viram novos `DbSet<T>` em `ApplicationDbContext`.
- Recuperação de senha por e-mail (o Identity já tem o mecanismo de token;
  falta só plugar um serviço de envio de e-mail).
- Perfil de Administrador, se for necessário (mesmo padrão: nova role +
  controller protegido).

## 7. Migrando para o Azure (quando a liberação sair)

O projeto já foi montado para trocar de banco sem mexer em código:

1. **Banco de dados**: crie um Azure SQL Database. No Azure, em
   **App Service → Configuração → Cadeias de conexão**, crie uma entrada do
   tipo `SQLAzure` chamada `DefaultConnection` com a connection string do
   Azure SQL. O ASP.NET Core lê isso automaticamente como se fosse o
   `ConnectionStrings:DefaultConnection` do `appsettings.json` — não precisa
   editar `Program.cs`.
   - Localmente, continue usando o LocalDB (`appsettings.json` já está
     configurado assim) — só a nuvem usa o Azure SQL.
   - Depois de apontar para o Azure SQL, rode `Update-Database` (ou configure
     migração automática/CI) para criar as tabelas lá também.
2. **Publicação**: botão direito no projeto → **Publicar** → **Azure** →
   **Azure App Service (Windows ou Linux)**. O assistente do Visual Studio
   cria o App Service (se ainda não existir) e já deixa configurado para
   publicar com um clique.
3. **Segredos**: nada de senha de banco no `appsettings.json` em produção —
   use as "Configurações do Aplicativo" do App Service (ou Azure Key Vault
   mais adiante).
4. Quando o time de infraestrutura responder, o único ajuste de fato é criar
   o recurso do Azure SQL/App Service e preencher essas configurações — o
   código deste projeto não muda.

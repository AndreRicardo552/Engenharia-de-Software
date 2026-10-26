# Tutorial de ADO.NET com C#: do básico ao avançado

Oct 24, 2026 · @Andre

## 1. Introdução

ADO.NET é a camada do .NET que permite ao seu programa C# conversar diretamente com um banco de dados: abrir uma conexão, enviar um comando SQL e ler o resultado. Tudo o que existe acima dele (Dapper, Entity Framework Core) usa ADO.NET por baixo, então entender esta camada é entender o que realmente acontece quando sua aplicação acessa dados.

**Para quem é este guia.** Para quem está começando em C# e .NET e nunca acessou banco de dados por código, e também para quem já usa EF Core e quer entender a base. Cada seção começa simples e vai aprofundando; as seções finais servem como consulta rápida.

**Como ler.**

- Iniciante total: leia as seções 1 a 9 em ordem, digitando os exemplos.
- Já sabe o básico: pule para 10 (async), 11 (transações) e 15 (PostgreSQL).
- Consulta rápida: vá direto à seção 20.

**Pré-requisitos.**

- Saber o básico de C#: variáveis, classes, métodos, `if`, `foreach`, listas.
- Ter o .NET SDK instalado (a seção 3 mostra como).
- Nenhum conhecimento de banco de dados é necessário: a seção 2 ensina o mínimo.

**Os dois bancos usados.**

| Banco | O que é | Quando usar | Pacote NuGet |
| --- | --- | --- | --- |
| SQLite | Banco inteiro dentro de um único arquivo, sem servidor | Estudo, apps desktop/mobile, testes, protótipos | `Microsoft.Data.Sqlite` |
| PostgreSQL | Servidor de banco de dados completo e robusto | Sistemas web, muitos usuários simultâneos, produção | `Npgsql` |

A grande vantagem do ADO.NET é que o código para os dois é quase idêntico. Muda o nome das classes (`SqliteConnection` vs `NpgsqlConnection`) e alguns detalhes, mas a forma de pensar é a mesma.

## 2. O mínimo de banco de dados e SQL

Um banco de dados relacional guarda informações em **tabelas**, parecidas com planilhas: cada **linha** é um registro (um produto, um cliente) e cada **coluna** é um campo (nome, preço). SQL é a linguagem usada para pedir coisas ao banco.

**Vocabulário essencial.**

| Termo | Significado | Exemplo |
| --- | --- | --- |
| Tabela | Conjunto de registros do mesmo tipo | `produtos` |
| Coluna | Um campo da tabela, com um tipo | `preco NUMERIC(10,2)` |
| Linha (registro) | Um item da tabela | o produto "Teclado" |
| Chave primária (PK) | Coluna que identifica cada linha de forma única | `id` |
| Chave estrangeira (FK) | Coluna que aponta para a PK de outra tabela | `pedidos.cliente_id` |
| NULL | Ausência de valor (não é zero nem texto vazio) | cliente sem telefone |

**Os quatro comandos que você vai usar 90% do tempo (CRUD).**

```sql
-- Create: inserir
INSERT INTO produtos (nome, preco) VALUES ('Teclado', 150.00);

-- Read: consultar
SELECT id, nome, preco FROM produtos WHERE preco > 100 ORDER BY nome;

-- Update: alterar
UPDATE produtos SET preco = 139.90 WHERE id = 1;

-- Delete: excluir
DELETE FROM produtos WHERE id = 1;
```

**Criando uma tabela.** A sintaxe muda um pouco entre os bancos, principalmente na coluna de id automático:

```sql
-- SQLite
CREATE TABLE IF NOT EXISTS produtos (
    id          INTEGER PRIMARY KEY AUTOINCREMENT,
    nome        TEXT    NOT NULL,
    preco       REAL    NOT NULL,
    estoque     INTEGER NOT NULL DEFAULT 0,
    criado_em   TEXT    NOT NULL
);

-- PostgreSQL
CREATE TABLE IF NOT EXISTS produtos (
    id          INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
    nome        VARCHAR(200)  NOT NULL,
    preco       NUMERIC(10,2) NOT NULL,
    estoque     INTEGER       NOT NULL DEFAULT 0,
    criado_em   TIMESTAMPTZ   NOT NULL DEFAULT now()
);
```

Cuidado: `UPDATE` e `DELETE` sem `WHERE` afetam **todas** as linhas da tabela. Sempre confira o `WHERE` antes de executar.

## 3. Preparando o ambiente

Você precisa de três coisas: o .NET SDK, um projeto console e os pacotes de cada banco. O PostgreSQL é opcional no início; dá para fazer quase todo o tutorial só com SQLite.

1. **Instale o .NET SDK.** Baixe em [dot.net](https://dotnet.microsoft.com/download) a versão LTS mais recente (no fim de 2026, o .NET 10). Confira no terminal:

   ```bash
   dotnet --version
   ```
2. **Crie o projeto.**

   ```bash
   dotnet new console -n EstudoAdoNet
   cd EstudoAdoNet
   ```
3. **Adicione os provedores (drivers) de banco.**

   ```bash
   dotnet add package Microsoft.Data.Sqlite
   dotnet add package Npgsql
   ```
4. **Suba um PostgreSQL com Docker** (a forma mais simples). Com o [Docker](https://www.docker.com/) instalado:

   ```bash
   docker run --name pg-estudo -e POSTGRES_PASSWORD=senha123 -e POSTGRES_DB=loja -p 5432:5432 -d postgres:18
   ```

   Isso cria um servidor em `localhost:5432`, usuário `postgres`, senha `senha123` e banco `loja`. Sem Docker, instale pelo [site oficial](https://www.postgresql.org/download/) e crie o banco `loja` com o pgAdmin.
5. **Instale um visualizador (opcional, mas ajuda muito).** [DB Browser for SQLite](https://sqlitebrowser.org/) para abrir arquivos `.db`; [DBeaver](https://dbeaver.io/) abre os dois bancos.

**Teste rápido.** Substitua o `Program.cs` por este código e rode `dotnet run`:

```csharp
using Microsoft.Data.Sqlite;

using var conexao = new SqliteConnection("Data Source=loja.db");
conexao.Open();
Console.WriteLine($"Conectado! Versão do SQLite: {conexao.ServerVersion}");
```

Se aparecer a versão, o ambiente está pronto. O arquivo `loja.db` foi criado na pasta de saída do programa.

## 4. Arquitetura do ADO.NET

O ADO.NET é um conjunto de **classes abstratas** no namespace `System.Data.Common` que cada banco implementa no seu **provedor** (o "driver"). Você aprende as classes base uma vez e usa o mesmo padrão em qualquer banco.

**As classes principais e seus equivalentes em cada provedor.**

| Papel | Classe base | SQLite | PostgreSQL |
| --- | --- | --- | --- |
| Conexão com o banco | `DbConnection` | `SqliteConnection` | `NpgsqlConnection` |
| Comando SQL a executar | `DbCommand` | `SqliteCommand` | `NpgsqlCommand` |
| Valor seguro enviado ao SQL | `DbParameter` | `SqliteParameter` | `NpgsqlParameter` |
| Leitor de resultados, linha a linha | `DbDataReader` | `SqliteDataReader` | `NpgsqlDataReader` |
| Transação | `DbTransaction` | `SqliteTransaction` | `NpgsqlTransaction` |
| Fábrica de objetos do provedor | `DbProviderFactory` | `SqliteFactory` | `NpgsqlFactory` |
| Preenche tabelas em memória | `DbDataAdapter` | (não existe) | `NpgsqlDataAdapter` |
| Fonte de conexões configurada | `DbDataSource` | (não existe) | `NpgsqlDataSource` |

**Os dois modos de trabalho.**

- **Conectado:** abre a conexão, executa, lê com o `DataReader` enquanto a conexão está aberta, fecha. É rápido, gasta pouca memória e é o modo usado no dia a dia.
- **Desconectado:** carrega os dados para objetos em memória (`DataTable`, `DataSet`), fecha a conexão e trabalha com a cópia. Útil em telas desktop e relatórios (seção 12).

**O fluxo de toda operação** é sempre o mesmo: criar a conexão, abrir, criar o comando, adicionar parâmetros, executar, ler o resultado (se houver) e liberar os recursos. O diagrama abaixo mostra como as peças se encaixam.

&#91;embedded content: arquitetura do ADO.NET · fluxo de uma operação\]

Seu código só fala com as classes base; o provedor escolhido (SQLite ou PostgreSQL) faz o trabalho específico de cada banco por baixo.

## 5. Conectando ao banco

A conexão é aberta a partir de uma **connection string**: um texto com pares `chave=valor` separados por `;` que diz onde está o banco e como entrar. Abra a conexão o mais tarde possível e feche o mais cedo possível.

**Connection strings de exemplo.**

| Banco | Connection string | Observação |
| --- | --- | --- |
| SQLite (arquivo) | `Data Source=loja.db` | Cria o arquivo se não existir |
| SQLite (só leitura) | `Data Source=loja.db;Mode=ReadOnly` | Modos: `ReadWriteCreate` (padrão), `ReadWrite`, `ReadOnly`, `Memory` |
| SQLite (em memória) | `Data Source=:memory:` | Some quando a conexão fecha; ótimo para testes |
| SQLite (com FKs) | `Data Source=loja.db;Foreign Keys=True` | Garante que chaves estrangeiras sejam verificadas |
| PostgreSQL | `Host=localhost;Port=5432;Database=loja;Username=postgres;Password=senha123` | Porta 5432 é o padrão |
| PostgreSQL (produção) | `Host=db.empresa.com;Database=loja;Username=app;Password=...;SSL Mode=Require;Maximum Pool Size=50` | SSL obrigatório e pool limitado |

**Abrindo e fechando com `using`.** Conexões usam recursos do sistema operacional e do servidor. O `using` garante que `Dispose()` seja chamado (e a conexão fechada) mesmo se ocorrer uma exceção:

```csharp
using Microsoft.Data.Sqlite;
using Npgsql;

// SQLite
using (var conSqlite = new SqliteConnection("Data Source=loja.db"))
{
    conSqlite.Open();
    Console.WriteLine($"SQLite: {conSqlite.State}"); // Open
} // aqui a conexão é fechada automaticamente

// PostgreSQL, com a sintaxe curta do using (C# 8+): fecha no fim do bloco/método
using var conPg = new NpgsqlConnection(
    "Host=localhost;Database=loja;Username=postgres;Password=senha123");
conPg.Open();
Console.WriteLine($"PostgreSQL {conPg.ServerVersion}: {conPg.State}");
```

**Montando a connection string com segurança.** Em vez de concatenar textos, use o builder do provedor, que trata caracteres especiais (como `;` dentro da senha):

```csharp
var sqliteCs = new SqliteConnectionStringBuilder
{
    DataSource = "loja.db",
    Mode = SqliteOpenMode.ReadWriteCreate,
    ForeignKeys = true
}.ToString();

var pgCs = new NpgsqlConnectionStringBuilder
{
    Host = "localhost",
    Database = "loja",
    Username = "postgres",
    Password = "senha123"
}.ToString();
```

**NpgsqlDataSource: a forma moderna no PostgreSQL.** Desde o Npgsql 7, o recomendado é criar **uma única** `NpgsqlDataSource` para a aplicação inteira e pedir conexões a ela:

```csharp
await using var dataSource = NpgsqlDataSource.Create(pgCs);

await using var conexao = await dataSource.OpenConnectionAsync();
// ou, sem gerenciar a conexão manualmente:
await using var cmd = dataSource.CreateCommand("SELECT version()");
Console.WriteLine(await cmd.ExecuteScalarAsync());
```

**Connection pooling (reaproveitamento de conexões).** Abrir uma conexão de verdade com o PostgreSQL é caro (rede, autenticação). Por isso o provedor mantém um **pool**: quando você chama `Dispose()`, a conexão física não é fechada, volta para o pool e é reaproveitada no próximo `Open()`. Consequências práticas:

- Abrir e fechar conexões a cada operação é **barato** e é o jeito certo. Não guarde uma conexão aberta numa variável global.
- Sempre feche (use `using`). Conexões esquecidas abertas esgotam o pool (o padrão do Npgsql é 100) e a aplicação trava esperando.
- O pool é separado por connection string exata. Use sempre a mesma string.
- O `Microsoft.Data.Sqlite` também tem pool (desde a versão 6), controlado por `Pooling=True/False`.

**Nunca deixe senhas no código-fonte.** Guarde a connection string em `appsettings.json`, variáveis de ambiente ou no *User Secrets* (seção 17).

## 6. Executando comandos

Todo SQL é enviado por um objeto **comando** (`DbCommand`), e ele tem três formas de execução. Escolha pela pergunta: "o que eu espero de volta?"

| Método | Retorna | Use para |
| --- | --- | --- |
| `ExecuteNonQuery()` | `int`: número de linhas afetadas | `INSERT`, `UPDATE`, `DELETE`, `CREATE TABLE` |
| `ExecuteScalar()` | `object?`: a primeira coluna da primeira linha | `COUNT(*)`, `MAX(...)`, id recém-inserido |
| `ExecuteReader()` | `DbDataReader`: várias linhas e colunas | `SELECT` em geral |

Todos têm versão assíncrona (`ExecuteNonQueryAsync` etc.), vista na seção 10.

**Criando o comando.** Há duas formas equivalentes:

```csharp
// 1) Pela conexão (recomendada: já liga o comando à conexão)
using var cmd = conexao.CreateCommand();
cmd.CommandText = "SELECT COUNT(*) FROM produtos";

// 2) Pelo construtor
using var cmd2 = new SqliteCommand("SELECT COUNT(*) FROM produtos", conexao);
```

**ExecuteNonQuery: criando a tabela e inserindo (SQLite).**

```csharp
using Microsoft.Data.Sqlite;

using var conexao = new SqliteConnection("Data Source=loja.db");
conexao.Open();

using (var criar = conexao.CreateCommand())
{
    criar.CommandText = """
        CREATE TABLE IF NOT EXISTS produtos (
            id        INTEGER PRIMARY KEY AUTOINCREMENT,
            nome      TEXT    NOT NULL,
            preco     REAL    NOT NULL,
            estoque   INTEGER NOT NULL DEFAULT 0,
            criado_em TEXT    NOT NULL
        );
        """;
    criar.ExecuteNonQuery();
}

using (var inserir = conexao.CreateCommand())
{
    inserir.CommandText = """
        INSERT INTO produtos (nome, preco, estoque, criado_em)
        VALUES ('Teclado', 150.0, 10, datetime('now'));
        """;
    int linhas = inserir.ExecuteNonQuery();
    Console.WriteLine($"{linhas} linha(s) inserida(s)"); // 1
}
```

O `"""..."""` é uma *raw string literal* (C# 11+): permite SQL de várias linhas sem escapar aspas. Os valores fixos acima são só para começar; a partir da seção 8 todo valor vem por **parâmetro**.

**A mesma coisa no PostgreSQL.** Repare que só mudam o tipo da conexão e o SQL de criação:

```csharp
using Npgsql;

using var conexao = new NpgsqlConnection(
    "Host=localhost;Database=loja;Username=postgres;Password=senha123");
conexao.Open();

using (var criar = conexao.CreateCommand())
{
    criar.CommandText = """
        CREATE TABLE IF NOT EXISTS produtos (
            id        INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
            nome      VARCHAR(200)  NOT NULL,
            preco     NUMERIC(10,2) NOT NULL,
            estoque   INTEGER       NOT NULL DEFAULT 0,
            criado_em TIMESTAMPTZ   NOT NULL DEFAULT now()
        );
        """;
    criar.ExecuteNonQuery();
}
```

**ExecuteScalar: um único valor.** O retorno é `object`, então converta. O tipo exato depende do banco: `COUNT(*)` vem como `long` nos dois.

```csharp
using var contar = conexao.CreateCommand();
contar.CommandText = "SELECT COUNT(*) FROM produtos";
long total = (long)contar.ExecuteScalar()!;

// Mais robusto quando o tipo pode variar entre bancos:
long total2 = Convert.ToInt64(contar.ExecuteScalar());
```

Se a consulta não retornar nenhuma linha, `ExecuteScalar` devolve `null`; se retornar uma linha com valor NULL, devolve `DBNull.Value`. Trate os dois casos quando isso for possível.

**ExecuteReader: várias linhas.** É o assunto da próxima seção.

## 7. Lendo dados com DataReader

O `DataReader` lê o resultado de um `SELECT` **uma linha por vez, só para frente**, como um cursor. Ele não carrega tudo na memória, por isso funciona bem até com milhões de linhas. A conexão precisa ficar aberta enquanto você lê.

**O laço básico.**

```csharp
using var cmd = conexao.CreateCommand();
cmd.CommandText = "SELECT id, nome, preco, estoque FROM produtos ORDER BY nome";

using var reader = cmd.ExecuteReader();
while (reader.Read()) // avança para a próxima linha; false quando acabar
{
    int id         = reader.GetInt32(0);   // coluna 0
    string nome    = reader.GetString(1);  // coluna 1
    decimal preco  = reader.GetDecimal(2);
    int estoque    = reader.GetInt32(3);

    Console.WriteLine($"{id} - {nome}: {preco:C} ({estoque} em estoque)");
}
```

**Formas de acessar uma coluna.**

| Forma | Exemplo | Comentário |
| --- | --- | --- |
| Por índice tipado | `reader.GetString(1)` | A mais rápida; quebra se a ordem do SELECT mudar |
| Por nome | `reader["nome"]` | Retorna `object`; precisa converter |
| Índice pelo nome | `reader.GetOrdinal("nome")` | Busque uma vez antes do laço e use o índice dentro dele |
| Genérica | `reader.GetFieldValue<decimal>(2)` | Funciona com qualquer tipo suportado pelo provedor |

Padrão recomendado, legível e rápido:

```csharp
using var reader = cmd.ExecuteReader();
int colId = reader.GetOrdinal("id");
int colNome = reader.GetOrdinal("nome");
int colPreco = reader.GetOrdinal("preco");

while (reader.Read())
{
    var id = reader.GetInt32(colId);
    var nome = reader.GetString(colNome);
    var preco = reader.GetDecimal(colPreco);
}
```

**Tratando NULL.** Chamar `GetString` numa coluna NULL lança `InvalidCastException`. Verifique antes com `IsDBNull`:

```csharp
string? descricao = reader.IsDBNull(colDescricao)
    ? null
    : reader.GetString(colDescricao);

// Ou, com a forma genérica e tipo anulável:
int? categoriaId = reader.IsDBNull(colCategoria)
    ? null
    : reader.GetFieldValue<int>(colCategoria);
```

Ao **enviar** NULL para o banco, use `DBNull.Value`, não `null` (seção 8).

**Mapeando para objetos.** Na prática você converte cada linha num objeto C# e devolve uma lista:

```csharp
public record Produto(int Id, string Nome, decimal Preco, int Estoque, DateTime CriadoEm);

static List<Produto> ListarProdutos(DbConnection conexao)
{
    using var cmd = conexao.CreateCommand();
    cmd.CommandText = "SELECT id, nome, preco, estoque, criado_em FROM produtos";

    var lista = new List<Produto>();
    using var r = cmd.ExecuteReader();
    while (r.Read())
    {
        lista.Add(new Produto(
            Id: r.GetInt32(0),
            Nome: r.GetString(1),
            Preco: r.GetDecimal(2),
            Estoque: r.GetInt32(3),
            CriadoEm: r.GetDateTime(4)));
    }
    return lista;
}
```

Como o parâmetro é do tipo base `DbConnection`, essa função serve tanto para `SqliteConnection` quanto para `NpgsqlConnection`.

**Correspondência de tipos mais comuns.**

| C# | SQLite (armazenamento) | PostgreSQL | Método de leitura |
| --- | --- | --- | --- |
| `int` | INTEGER | `integer` | `GetInt32` |
| `long` | INTEGER | `bigint` | `GetInt64` |
| `decimal` | REAL ou TEXT | `numeric` | `GetDecimal` |
| `double` | REAL | `double precision` | `GetDouble` |
| `string` | TEXT | `text`, `varchar` | `GetString` |
| `bool` | INTEGER (0/1) | `boolean` | `GetBoolean` |
| `DateTime` | TEXT (ISO 8601) | `timestamp`, `timestamptz` | `GetDateTime` |
| `Guid` | TEXT ou BLOB | `uuid` | `GetGuid` |
| `byte[]` | BLOB | `bytea` | `GetFieldValue<byte[]>` |

No SQLite, `REAL` é ponto flutuante e pode gerar arredondamentos em valores monetários. Para dinheiro, uma alternativa comum é guardar centavos como `INTEGER`.

**Vários resultados num só comando.** Se o SQL tiver dois `SELECT` separados por `;`, use `NextResult()` para passar ao segundo conjunto:

```csharp
cmd.CommandText = "SELECT COUNT(*) FROM produtos; SELECT nome FROM produtos LIMIT 3;";
using var r = cmd.ExecuteReader();
r.Read();
Console.WriteLine($"Total: {r.GetInt64(0)}");

r.NextResult();
while (r.Read())
    Console.WriteLine(r.GetString(0));
```

## 8. Parâmetros e SQL Injection

**Regra de ouro: nunca monte SQL concatenando valores digitados pelo usuário. Use sempre parâmetros.** Esta é a seção mais importante do tutorial para a segurança da sua aplicação.

**O problema.** Imagine uma busca por nome feita assim:

```csharp
// ERRADO: vulnerável a SQL Injection
string nome = Console.ReadLine()!;
cmd.CommandText = $"SELECT * FROM produtos WHERE nome = '{nome}'";
```

Se o usuário digitar `x' OR '1'='1`, o SQL vira `SELECT * FROM produtos WHERE nome = 'x' OR '1'='1'` e devolve a tabela inteira. Com `x'; DROP TABLE produtos; --` o estrago pode ser pior. Isso se chama **SQL Injection** e é uma das falhas de segurança mais exploradas na web.

**A solução: parâmetros.** O SQL leva um marcador (`@nome`) e o valor vai separado. O banco trata o valor sempre como dado, nunca como parte do comando:

```csharp
// CERTO
cmd.CommandText = "SELECT id, nome, preco FROM produtos WHERE nome = @nome";
cmd.Parameters.AddWithValue("@nome", nome);
```

Benefícios extras: o banco pode reaproveitar o plano de execução, datas e decimais são convertidos corretamente (sem problema de vírgula/ponto) e o código fica mais limpo.

**Formas de adicionar parâmetros.**

```csharp
// 1) AddWithValue: curta, o tipo é deduzido do valor C#
cmd.Parameters.AddWithValue("@preco", 99.90m);

// 2) Especificando o tipo (mais controle)
//    SQLite
cmd.Parameters.Add("@nome", SqliteType.Text).Value = "Mouse";
//    PostgreSQL
cmd.Parameters.Add("@nome", NpgsqlTypes.NpgsqlDbType.Varchar).Value = "Mouse";

// 3) Genérica e independente de banco (funciona com DbCommand)
var p = cmd.CreateParameter();
p.ParameterName = "@estoque";
p.Value = 25;
p.DbType = System.Data.DbType.Int32;
cmd.Parameters.Add(p);

// 4) Npgsql: parâmetro fortemente tipado, sem conversão para object (boxing)
cmd.Parameters.Add(new NpgsqlParameter<int> { ParameterName = "estoque", TypedValue = 25 });
```

**Enviando NULL.** `null` do C# não vale como valor de parâmetro; use `DBNull.Value`:

```csharp
string? descricao = null;
cmd.Parameters.AddWithValue("@descricao", (object?)descricao ?? DBNull.Value);
```

**Sintaxe dos marcadores em cada provedor.**

| Provedor | Marcadores aceitos | Exemplo |
| --- | --- | --- |
| Microsoft.Data.Sqlite | `@nome`, `$nome`, `:nome` | `WHERE id = @id` |
| Npgsql (nomeados) | `@nome` | `WHERE id = @id` |
| Npgsql (posicionais) | `$1`, `$2`... com parâmetros sem nome, na ordem | `WHERE id = $1` |

**O que NÃO pode ser parâmetro.** Parâmetros substituem apenas **valores**. Nomes de tabela, nomes de coluna, `ASC`/`DESC` e palavras-chave não podem ser parametrizados. Quando precisar deles dinâmicos (ex.: ordenação escolhida pelo usuário), valide contra uma lista fixa:

```csharp
var colunasPermitidas = new Dictionary<string, string>
{
    ["nome"] = "nome",
    ["preco"] = "preco",
};

if (!colunasPermitidas.TryGetValue(ordenarPor, out var coluna))
    coluna = "nome"; // valor padrão seguro

cmd.CommandText = $"SELECT id, nome, preco FROM produtos ORDER BY {coluna}";
```

**Busca com LIKE.** O curinga `%` vai no **valor**, não no SQL:

```csharp
cmd.CommandText = "SELECT nome FROM produtos WHERE nome LIKE @termo";
cmd.Parameters.AddWithValue("@termo", $"%{termo}%");
```

No PostgreSQL, `ILIKE` faz a mesma busca ignorando maiúsculas e minúsculas.

## 9. CRUD completo passo a passo

Esta seção junta tudo numa classe `ProdutoRepositorio` que insere, lista, busca, atualiza e exclui produtos, e que funciona **sem alteração** em SQLite e PostgreSQL. O truque é receber uma função que cria a conexão e programar contra as classes base (`DbConnection`, `DbCommand`).

**O modelo.**

```csharp
public record Produto(int Id, string Nome, decimal Preco, int Estoque, DateTime CriadoEm);
```

**O repositório.**

```csharp
using System.Data.Common;

public class ProdutoRepositorio
{
    private readonly Func<DbConnection> _criarConexao;

    public ProdutoRepositorio(Func<DbConnection> criarConexao)
        => _criarConexao = criarConexao;

    // Abre uma conexão nova a cada operação (o pool torna isso barato)
    private DbConnection Abrir()
    {
        var con = _criarConexao();
        con.Open();
        return con;
    }

    // Adiciona parâmetro de forma independente de banco
    private static void Param(DbCommand cmd, string nome, object? valor)
    {
        var p = cmd.CreateParameter();
        p.ParameterName = nome;
        p.Value = valor ?? DBNull.Value;
        cmd.Parameters.Add(p);
    }

    // CREATE: insere e devolve o id gerado pelo banco
    public int Inserir(string nome, decimal preco, int estoque)
    {
        using var con = Abrir();
        using var cmd = con.CreateCommand();
        // RETURNING funciona no PostgreSQL e no SQLite (3.35+)
        cmd.CommandText = """
            INSERT INTO produtos (nome, preco, estoque, criado_em)
            VALUES (@nome, @preco, @estoque, @criadoEm)
            RETURNING id
            """;
        Param(cmd, "@nome", nome);
        Param(cmd, "@preco", preco);
        Param(cmd, "@estoque", estoque);
        Param(cmd, "@criadoEm", DateTime.UtcNow);
        return Convert.ToInt32(cmd.ExecuteScalar());
    }

    // READ (todos)
    public List<Produto> Listar()
    {
        using var con = Abrir();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT id, nome, preco, estoque, criado_em FROM produtos ORDER BY nome";

        var lista = new List<Produto>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            lista.Add(Mapear(r));
        return lista;
    }

    // READ (um)
    public Produto? ObterPorId(int id)
    {
        using var con = Abrir();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "SELECT id, nome, preco, estoque, criado_em FROM produtos WHERE id = @id";
        Param(cmd, "@id", id);

        using var r = cmd.ExecuteReader();
        return r.Read() ? Mapear(r) : null;
    }

    // UPDATE: devolve true se alguma linha foi alterada
    public bool Atualizar(Produto p)
    {
        using var con = Abrir();
        using var cmd = con.CreateCommand();
        cmd.CommandText = """
            UPDATE produtos
               SET nome = @nome, preco = @preco, estoque = @estoque
             WHERE id = @id
            """;
        Param(cmd, "@nome", p.Nome);
        Param(cmd, "@preco", p.Preco);
        Param(cmd, "@estoque", p.Estoque);
        Param(cmd, "@id", p.Id);
        return cmd.ExecuteNonQuery() > 0;
    }

    // DELETE
    public bool Excluir(int id)
    {
        using var con = Abrir();
        using var cmd = con.CreateCommand();
        cmd.CommandText = "DELETE FROM produtos WHERE id = @id";
        Param(cmd, "@id", id);
        return cmd.ExecuteNonQuery() > 0;
    }

    private static Produto Mapear(DbDataReader r) => new(
        Id: r.GetInt32(0),
        Nome: r.GetString(1),
        Preco: r.GetDecimal(2),
        Estoque: r.GetInt32(3),
        CriadoEm: r.GetDateTime(4));
}
```

**Usando com SQLite.**

```csharp
using Microsoft.Data.Sqlite;

var repo = new ProdutoRepositorio(() => new SqliteConnection("Data Source=loja.db"));

int id = repo.Inserir("Monitor 24\"", 899.90m, 5);
Console.WriteLine($"Inserido com id {id}");

var monitor = repo.ObterPorId(id)!;
repo.Atualizar(monitor with { Preco = 849.90m }); // 'with' cria cópia alterada do record

foreach (var p in repo.Listar())
    Console.WriteLine($"{p.Id,3} | {p.Nome,-20} | {p.Preco,10:C} | {p.Estoque}");

repo.Excluir(id);
```

**Usando com PostgreSQL.** Só muda a linha que cria a conexão:

```csharp
using Npgsql;

var cs = "Host=localhost;Database=loja;Username=postgres;Password=senha123";
var repo = new ProdutoRepositorio(() => new NpgsqlConnection(cs));
```

Esse é o poder do ADO.NET programado contra as classes base: a lógica é escrita uma vez e o banco vira um detalhe de configuração. As diferenças que sobram (tipos de coluna, sintaxe do `CREATE TABLE`) ficam nos scripts de criação.

## 10. Programação assíncrona (async/await)

Em aplicações web e de interface gráfica, use sempre os métodos `...Async`. Enquanto o banco processa a consulta, a thread fica livre para atender outras requisições ou manter a tela respondendo, em vez de ficar parada esperando.

**Equivalências.**

| Síncrono | Assíncrono |
| --- | --- |
| `conexao.Open()` | `await conexao.OpenAsync()` |
| `cmd.ExecuteNonQuery()` | `await cmd.ExecuteNonQueryAsync()` |
| `cmd.ExecuteScalar()` | `await cmd.ExecuteScalarAsync()` |
| `cmd.ExecuteReader()` | `await cmd.ExecuteReaderAsync()` |
| `reader.Read()` | `await reader.ReadAsync()` |
| `conexao.BeginTransaction()` | `await conexao.BeginTransactionAsync()` |
| `using var x = ...` | `await using var x = ...` |

**Exemplo: listagem assíncrona com cancelamento.**

```csharp
public async Task<List<Produto>> ListarAsync(CancellationToken ct = default)
{
    await using var con = _criarConexao();
    await con.OpenAsync(ct);

    await using var cmd = con.CreateCommand();
    cmd.CommandText = "SELECT id, nome, preco, estoque, criado_em FROM produtos ORDER BY nome";

    var lista = new List<Produto>();
    await using var r = await cmd.ExecuteReaderAsync(ct);
    while (await r.ReadAsync(ct))
        lista.Add(Mapear(r));

    return lista;
}
```

**O CancellationToken.** Permite interromper uma consulta demorada: se o usuário fechar a página ou clicar em "Cancelar", o token é sinalizado e o provedor tenta cancelar o comando no servidor. Em ASP.NET Core, receba o token como parâmetro da action e repasse até o ADO.NET.

```csharp
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
try
{
    var produtos = await repo.ListarAsync(cts.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("A consulta demorou demais e foi cancelada.");
}
```

**Streaming com IAsyncEnumerable.** Para resultados grandes, entregue item por item em vez de montar uma lista enorme na memória:

```csharp
public async IAsyncEnumerable<Produto> StreamAsync(
    [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
{
    await using var con = _criarConexao();
    await con.OpenAsync(ct);
    await using var cmd = con.CreateCommand();
    cmd.CommandText = "SELECT id, nome, preco, estoque, criado_em FROM produtos";

    await using var r = await cmd.ExecuteReaderAsync(ct);
    while (await r.ReadAsync(ct))
        yield return Mapear(r);
}

// Consumo
await foreach (var p in repo.StreamAsync())
    Console.WriteLine(p.Nome);
```

**Tempo limite do comando.** `cmd.CommandTimeout` define em segundos quanto o comando pode durar (padrão de 30 s em ambos os provedores; 0 significa sem limite).

**Cuidados.**

- Não misture: dentro de um método `async`, use os métodos `Async` de ponta a ponta. Evite `.Result` e `.Wait()`, que podem travar a aplicação.
- No `Microsoft.Data.Sqlite`, os métodos `Async` existem mas executam de forma síncrona por baixo, porque o SQLite é um arquivo local sem rede. Use-os mesmo assim, pela consistência do código.
- Uma conexão executa **um comando por vez**. Não dispare duas consultas em paralelo na mesma conexão; use uma conexão para cada tarefa paralela.

## 11. Transações

Uma transação agrupa vários comandos num bloco **tudo ou nada**: ou todos são gravados (`Commit`), ou nenhum é (`Rollback`). Use sempre que uma operação de negócio exigir mais de um comando que precisa ficar consistente.

**Exemplo clássico: registrar uma venda.** Vender um produto exige baixar o estoque **e** gravar a venda. Se o programa cair entre os dois comandos sem transação, o estoque some e a venda não existe.

```sql
CREATE TABLE IF NOT EXISTS vendas (
    id          INTEGER PRIMARY KEY AUTOINCREMENT, -- PostgreSQL: GENERATED ALWAYS AS IDENTITY
    produto_id  INTEGER NOT NULL REFERENCES produtos(id),
    quantidade  INTEGER NOT NULL,
    vendido_em  TEXT    NOT NULL                   -- PostgreSQL: TIMESTAMPTZ
);
```

```csharp
public async Task RegistrarVendaAsync(int produtoId, int quantidade, CancellationToken ct = default)
{
    await using var con = _criarConexao();
    await con.OpenAsync(ct);
    await using var tx = await con.BeginTransactionAsync(ct);

    try
    {
        // 1) Baixa o estoque só se houver quantidade suficiente
        await using (var baixa = con.CreateCommand())
        {
            baixa.Transaction = tx; // associe SEMPRE o comando à transação
            baixa.CommandText = """
                UPDATE produtos SET estoque = estoque - @qtd
                 WHERE id = @id AND estoque >= @qtd
                """;
            Param(baixa, "@qtd", quantidade);
            Param(baixa, "@id", produtoId);

            if (await baixa.ExecuteNonQueryAsync(ct) == 0)
                throw new InvalidOperationException("Estoque insuficiente ou produto inexistente.");
        }

        // 2) Registra a venda
        await using (var venda = con.CreateCommand())
        {
            venda.Transaction = tx;
            venda.CommandText = """
                INSERT INTO vendas (produto_id, quantidade, vendido_em)
                VALUES (@id, @qtd, @quando)
                """;
            Param(venda, "@id", produtoId);
            Param(venda, "@qtd", quantidade);
            Param(venda, "@quando", DateTime.UtcNow);
            await venda.ExecuteNonQueryAsync(ct);
        }

        await tx.CommitAsync(ct); // grava tudo
    }
    catch
    {
        await tx.RollbackAsync(ct); // desfaz tudo
        throw;                      // repassa o erro para quem chamou
    }
}
```

Se a transação for descartada (`Dispose`) sem `Commit`, o provedor faz `Rollback` automaticamente. O `catch` explícito acima deixa a intenção clara.

**Associe o comando à transação.** O `Microsoft.Data.Sqlite` exige `cmd.Transaction = tx` e lança exceção sem isso; o Npgsql não exige. Defina sempre, para o código funcionar nos dois.

**Savepoints: desfazer só uma parte.** Dentro de uma transação você pode marcar pontos e voltar até eles, sem perder o restante:

```csharp
await tx.SaveAsync("antes_do_brinde");
try
{
    // ... tenta adicionar um brinde
}
catch
{
    await tx.RollbackAsync("antes_do_brinde"); // desfaz só o brinde
}
await tx.CommitAsync();
```

**Níveis de isolamento.** Definem o quanto uma transação "enxerga" das alterações de outras que rodam ao mesmo tempo.

| Nível | O que garante | Observação |
| --- | --- | --- |
| `ReadCommitted` | Só lê dados já confirmados | Padrão do PostgreSQL; bom para a maioria dos casos |
| `RepeatableRead` | Reler a mesma linha dá o mesmo resultado | No PostgreSQL, pode falhar com erro de serialização e exigir nova tentativa |
| `Serializable` | Equivale a executar as transações uma de cada vez | O mais seguro e o mais sujeito a conflitos; é o comportamento normal do SQLite |

```csharp
await using var tx = await con.BeginTransactionAsync(System.Data.IsolationLevel.Serializable, ct);
```

**Boas práticas.**

- Mantenha transações **curtas**: nunca espere entrada do usuário ou chamadas HTTP externas com uma transação aberta, pois ela segura bloqueios no banco.
- No SQLite, só uma transação de escrita acontece por vez no arquivo inteiro. Escritas concorrentes esperam ou falham com `database is locked` (veja a seção 14).
- Prefira condições no próprio SQL (`WHERE estoque >= @qtd`) a ler, conferir no C# e depois gravar: entre a leitura e a escrita, outro usuário pode ter alterado o dado.

## 12. Modo desconectado: DataTable, DataSet e DataAdapter

No modo desconectado você copia os dados para objetos em memória, fecha a conexão e trabalha com a cópia. É o modelo original do ADO.NET, ainda útil para grids em Windows Forms/WPF, relatórios e exportações. Em APIs web modernas, prefira o `DataReader` mapeando para classes.

| Classe | O que representa |
| --- | --- |
| `DataTable` | Uma tabela em memória: colunas (`DataColumn`) e linhas (`DataRow`) |
| `DataSet` | Um conjunto de `DataTable`s, com relações entre elas |
| `DataAdapter` | Ponte entre o banco e o `DataSet`: `Fill` lê, `Update` grava as alterações |

**Carregando uma DataTable (funciona nos dois bancos).** O método `DataTable.Load` consome um `DataReader` inteiro:

```csharp
using System.Data;

using var cmd = conexao.CreateCommand();
cmd.CommandText = "SELECT id, nome, preco FROM produtos";

var tabela = new DataTable("produtos");
using (var r = cmd.ExecuteReader())
    tabela.Load(r);
// a partir daqui a conexão pode ser fechada

foreach (DataRow linha in tabela.Rows)
{
    var nome = linha.Field<string>("nome");
    // Convert: no SQLite a coluna REAL chega como double; no PostgreSQL, como decimal
    var preco = Convert.ToDecimal(linha["preco"]);
    Console.WriteLine($"{nome}: {preco:C}");
}

// Filtrar e ordenar em memória
DataRow[] caros = tabela.Select("preco > 500", "nome ASC");
```

**DataAdapter com PostgreSQL.** O Npgsql tem `NpgsqlDataAdapter`, que lê com `Fill` e grava de volta as linhas alteradas com `Update`. O `NpgsqlCommandBuilder` gera os comandos de `INSERT`, `UPDATE` e `DELETE` a partir do `SELECT` (a tabela precisa de chave primária):

```csharp
using Npgsql;
using System.Data;

using var con = new NpgsqlConnection(cs);
var adapter = new NpgsqlDataAdapter("SELECT id, nome, preco, estoque FROM produtos", con);
var builder = new NpgsqlCommandBuilder(adapter); // gera Insert/Update/Delete

var ds = new DataSet();
adapter.Fill(ds, "produtos"); // abre e fecha a conexão sozinho

var tabela = ds.Tables["produtos"]!;
tabela.Rows[0]["preco"] = 199.90m;         // altera
tabela.Rows[1].Delete();                     // marca para excluir

int alteradas = adapter.Update(ds, "produtos"); // envia só o que mudou
Console.WriteLine($"{alteradas} linha(s) sincronizada(s)");
```

**E no SQLite?** O `Microsoft.Data.Sqlite` **não** tem `DataAdapter`. Para ler, use `DataTable.Load` (acima). Para gravar, percorra as linhas alteradas com `tabela.GetChanges()` e execute os comandos você mesmo, ou use o pacote alternativo `System.Data.SQLite`, que oferece `SQLiteDataAdapter`.

**Estado das linhas.** Cada `DataRow` sabe o que aconteceu com ela desde a carga, pela propriedade `RowState`: `Unchanged`, `Added`, `Modified` ou `Deleted`. É isso que o `Update` usa para decidir qual comando executar. Depois de gravar, `tabela.AcceptChanges()` marca tudo como `Unchanged`.

## 13. Código independente de banco

A seção 9 já mostrou a técnica principal: programar contra `DbConnection` e receber a criação da conexão de fora. Esta seção mostra como escolher o banco **por configuração**, sem recompilar.

**DbProviderFactory.** Cada provedor expõe uma fábrica (`SqliteFactory.Instance`, `NpgsqlFactory.Instance`) que cria conexões, comandos e parâmetros do tipo certo:

```csharp
using System.Data.Common;
using Microsoft.Data.Sqlite;
using Npgsql;

DbProviderFactory fabrica = config.Banco switch
{
    "sqlite"   => SqliteFactory.Instance,
    "postgres" => NpgsqlFactory.Instance,
    _ => throw new NotSupportedException(config.Banco)
};

using var con = fabrica.CreateConnection()!;
con.ConnectionString = config.ConnectionString;
con.Open();

using var cmd = con.CreateCommand();
cmd.CommandText = "SELECT COUNT(*) FROM produtos";
Console.WriteLine(cmd.ExecuteScalar());
```

**Registro por nome (DbProviderFactories).** No .NET moderno os provedores não são registrados automaticamente. Registre uma vez na inicialização e depois busque pelo nome que veio da configuração:

```csharp
DbProviderFactories.RegisterFactory("Microsoft.Data.Sqlite", SqliteFactory.Instance);
DbProviderFactories.RegisterFactory("Npgsql", NpgsqlFactory.Instance);

var fabrica = DbProviderFactories.GetFactory(config.Provedor); // "Npgsql", por exemplo
```

**DbDataSource (.NET 7+).** É a abstração mais nova: um objeto único, criado na inicialização, que entrega conexões e comandos já configurados. O Npgsql implementa com `NpgsqlDataSource`. Para o SQLite, que não tem implementação própria, o caminho é a fábrica acima.

```csharp
DbDataSource fonte = NpgsqlDataSource.Create(cs);
await using var con = await fonte.OpenConnectionAsync();
```

**Onde a portabilidade termina.** As classes são as mesmas, mas o **SQL** não é 100% igual entre bancos. Diferenças que você vai encontrar:

| Assunto | SQLite | PostgreSQL |
| --- | --- | --- |
| Id automático | `INTEGER PRIMARY KEY AUTOINCREMENT` | `GENERATED ALWAYS AS IDENTITY` |
| Data/hora atual | `datetime('now')` | `now()` |
| Limitar linhas | `LIMIT 10 OFFSET 20` | `LIMIT 10 OFFSET 20` (igual) |
| Busca sem diferenciar maiúsculas | `LIKE` já ignora para letras ASCII | `ILIKE` |
| Inserir ou atualizar (upsert) | `INSERT ... ON CONFLICT(id) DO UPDATE` | `INSERT ... ON CONFLICT (id) DO UPDATE` (igual) |
| Tipos de data, decimal, booleano | Não existem de verdade; guardados como TEXT, REAL ou INTEGER | Tipos nativos e rigorosos |
| Concorrência de escrita | Uma escrita por vez no arquivo | Muitas escritas simultâneas |

Estratégia prática: escreva o SQL comum (o "ANSI" que os dois aceitam) sempre que possível e isole o específico em métodos ou arquivos por banco. Se o projeto realmente precisa suportar vários bancos com SQL muito diferente, um ORM como o EF Core (seção 19) pode compensar.

## 14. Recursos específicos do SQLite

O SQLite é um arquivo, não um servidor, e isso muda algumas regras: tipos são flexíveis, só uma escrita acontece por vez e a configuração é feita com `PRAGMA`s. Estes são os recursos que mais fazem diferença no dia a dia.

**Inserções em massa: transação + comando reaproveitado.** Sem transação, cada `INSERT` grava no disco separadamente e mil inserções podem levar segundos. Dentro de uma transação, reaproveitando o mesmo comando e só trocando os valores, ficam muito mais rápidas:

```csharp
using var con = new SqliteConnection("Data Source=loja.db");
con.Open();
using var tx = con.BeginTransaction();

using var cmd = con.CreateCommand();
cmd.Transaction = tx;
cmd.CommandText = "INSERT INTO produtos (nome, preco, estoque, criado_em) VALUES ($nome, $preco, $estoque, $data)";
var pNome = cmd.Parameters.Add("$nome", SqliteType.Text);
var pPreco = cmd.Parameters.Add("$preco", SqliteType.Real);
var pEstoque = cmd.Parameters.Add("$estoque", SqliteType.Integer);
var pData = cmd.Parameters.Add("$data", SqliteType.Text);

for (int i = 1; i <= 10_000; i++)
{
    pNome.Value = $"Produto {i}";
    pPreco.Value = i * 1.5;
    pEstoque.Value = i % 50;
    pData.Value = DateTime.UtcNow;
    cmd.ExecuteNonQuery();
}

tx.Commit();
```

**Modo WAL: leitores não bloqueiam o escritor.** Por padrão, uma escrita bloqueia leituras no arquivo. O modo WAL (*Write-Ahead Logging*) permite ler enquanto alguém grava. A configuração fica salva no arquivo, então basta executar uma vez:

```csharp
using var cmd = con.CreateCommand();
cmd.CommandText = "PRAGMA journal_mode = WAL;";
cmd.ExecuteNonQuery();
```

**"database is locked".** Acontece quando duas conexões tentam escrever ao mesmo tempo. O `Microsoft.Data.Sqlite` repete a tentativa automaticamente até o tempo limite do comando (`Default Timeout` na connection string, em segundos). Para reduzir o problema: ative WAL, mantenha transações curtas e sempre feche conexões com `using`.

**PRAGMAs úteis.**

| PRAGMA | Para que serve |
| --- | --- |
| `PRAGMA foreign_keys = ON;` | Liga a verificação de chaves estrangeiras na conexão (ou use `Foreign Keys=True` na connection string) |
| `PRAGMA journal_mode = WAL;` | Melhora a concorrência entre leitura e escrita |
| `PRAGMA table_info(produtos);` | Lista as colunas de uma tabela |
| `PRAGMA integrity_check;` | Verifica se o arquivo está corrompido |
| `PRAGMA user_version = 3;` | Guarda um número livre no arquivo, útil para controlar a versão do esquema (migrações) |

**Banco em memória compartilhado.** `Data Source=:memory:` cria um banco que só existe naquela conexão. Para várias conexões enxergarem o mesmo banco em memória, use um nome com cache compartilhado e mantenha **uma conexão aberta** o tempo todo (quando a última fecha, o banco some):

```csharp
const string csMemoria = "Data Source=TesteCompartilhado;Mode=Memory;Cache=Shared";
using var guardia = new SqliteConnection(csMemoria);
guardia.Open(); // mantém o banco vivo

using var outra = new SqliteConnection(csMemoria);
outra.Open(); // enxerga as mesmas tabelas
```

**Último id inserido.** Além do `RETURNING id`, existe a função `last_insert_rowid()`, válida na mesma conexão logo após o `INSERT`:

```csharp
cmd.CommandText = "INSERT INTO produtos (nome, preco, estoque, criado_em) VALUES ('Cabo', 20, 1, datetime('now')); SELECT last_insert_rowid();";
long id = (long)cmd.ExecuteScalar()!;
```

**Funções C# dentro do SQL.** Você pode registrar uma função C# e chamá-la nas consultas daquela conexão:

```csharp
con.CreateFunction("sem_acento", (string texto) =>
    texto.Normalize(System.Text.NormalizationForm.FormD)
         .Where(c => System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c)
                     != System.Globalization.UnicodeCategory.NonSpacingMark)
         .Aggregate("", (s, c) => s + c));

cmd.CommandText = "SELECT nome FROM produtos WHERE sem_acento(nome) LIKE '%cafe%'";
```

**Tipos dinâmicos.** No SQLite o tipo pertence ao **valor**, não à coluna: uma coluna `INTEGER` aceita texto sem reclamar. Para impedir isso, crie a tabela com `STRICT` (SQLite 3.37+), que passa a exigir os tipos declarados (`INTEGER`, `REAL`, `TEXT`, `BLOB`, `ANY`).

**Criptografia.** O SQLite padrão não criptografa o arquivo. Para isso existe o SQLCipher, usado trocando o pacote Microsoft.Data.Sqlite por Microsoft.Data.Sqlite.Core mais o pacote `SQLitePCLRaw.bundle_e_sqlcipher` e a chave `Password=` na connection string.

## 15. Recursos específicos do PostgreSQL (Npgsql)

O PostgreSQL tem tipos e comandos que vão muito além do SQL básico, e o Npgsql dá acesso a quase todos. Os exemplos abaixo assumem uma `NpgsqlDataSource` chamada `dataSource` (seção 5).

**Datas e fuso horário: a regra que mais confunde.** No Npgsql 6+, um `DateTime` com `Kind = Utc` é enviado como `timestamptz` (com fuso) e um com `Kind` Local ou Unspecified é enviado como `timestamp` (sem fuso). Ao ler uma coluna `timestamptz`, você recebe um `DateTime` em UTC. Misturar os dois gera horários deslocados, ou erro se o parâmetro for forçado como `NpgsqlDbType.TimestampTz` com um valor que não seja UTC. Recomendação: use colunas `timestamptz`, grave sempre `DateTime.UtcNow` e converta para o horário local só na hora de exibir.

```csharp
Param(cmd, "@quando", DateTime.UtcNow);          // certo: vai como timestamptz
Param(cmd, "@quando", DateTime.Now);             // evite: vai como timestamp sem fuso
var local = r.GetDateTime(0).ToLocalTime();       // para exibir
```

**Upsert: inserir ou atualizar.**

```sql
INSERT INTO produtos (id, nome, preco, estoque)
OVERRIDING SYSTEM VALUE
VALUES (@id, @nome, @preco, @estoque)
ON CONFLICT (id) DO UPDATE
   SET nome = EXCLUDED.nome, preco = EXCLUDED.preco, estoque = EXCLUDED.estoque;
```

`EXCLUDED` representa a linha que você tentou inserir. O `OVERRIDING SYSTEM VALUE` só é necessário porque a coluna `id` foi criada como `GENERATED ALWAYS`.

**Arrays: buscar vários ids com um único parâmetro.** Arrays C# viram arrays do PostgreSQL automaticamente:

```csharp
await using var cmd = dataSource.CreateCommand(
    "SELECT id, nome FROM produtos WHERE id = ANY(@ids)");
cmd.Parameters.AddWithValue("ids", new[] { 1, 5, 9 });
```

Isso substitui o `IN (...)` montado na mão, que seria vulnerável a SQL Injection. Colunas também podem ser arrays: `tags TEXT[]`, lidas com `r.GetFieldValue<string[]>(i)`.

**JSONB: dados semiestruturados.**

```sql
ALTER TABLE produtos ADD COLUMN atributos JSONB;
```

```csharp
using System.Text.Json;
using NpgsqlTypes;

var atributos = new { cor = "preto", voltagem = 110 };

await using var cmd = dataSource.CreateCommand(
    "UPDATE produtos SET atributos = @atributos WHERE id = @id");
cmd.Parameters.Add(new NpgsqlParameter("atributos", NpgsqlDbType.Jsonb)
{
    Value = JsonSerializer.Serialize(atributos)
});
cmd.Parameters.AddWithValue("id", 1);
await cmd.ExecuteNonQueryAsync();

// Consultar por um campo do JSON: ->> devolve o valor como texto
await using var busca = dataSource.CreateCommand(
    "SELECT nome, atributos FROM produtos WHERE atributos->>'cor' = @cor");
busca.Parameters.AddWithValue("cor", "preto");
await using var r = await busca.ExecuteReaderAsync();
while (await r.ReadAsync())
{
    var json = r.GetString(1); // JSONB pode ser lido como string
    var doc = JsonDocument.Parse(json);
}
```

**COPY binário: importação em massa muito rápida.** Para carregar milhares ou milhões de linhas, o `COPY` é muito mais rápido que `INSERT`s individuais:

```csharp
await using var con = await dataSource.OpenConnectionAsync();
await using var writer = await con.BeginBinaryImportAsync(
    "COPY produtos (nome, preco, estoque) FROM STDIN (FORMAT BINARY)");

foreach (var p in produtosParaImportar)
{
    await writer.StartRowAsync();
    await writer.WriteAsync(p.Nome, NpgsqlDbType.Varchar);
    await writer.WriteAsync(p.Preco, NpgsqlDbType.Numeric);
    await writer.WriteAsync(p.Estoque, NpgsqlDbType.Integer);
}

ulong total = await writer.CompleteAsync(); // sem CompleteAsync, nada é gravado
Console.WriteLine($"{total} linhas importadas");
```

**Batching: vários comandos numa única ida ao servidor.** Cada comando normalmente faz uma viagem de rede. Com `NpgsqlBatch` (implementação do `DbBatch` do .NET 6+), vários vão juntos:

```csharp
await using var batch = dataSource.CreateBatch();
batch.BatchCommands.Add(new NpgsqlBatchCommand("UPDATE produtos SET estoque = estoque - 1 WHERE id = $1")
{
    Parameters = { new() { Value = 3 } } // parâmetro posicional $1
});
batch.BatchCommands.Add(new NpgsqlBatchCommand("SELECT COUNT(*) FROM produtos WHERE estoque = 0"));

await using var r = await batch.ExecuteReaderAsync();
await r.ReadAsync();
Console.WriteLine($"Produtos zerados: {r.GetInt64(0)}");
```

**LISTEN/NOTIFY: avisos em tempo real.** O PostgreSQL pode notificar sua aplicação quando algo acontece, sem ficar consultando a tabela repetidamente:

```csharp
await using var con = await dataSource.OpenConnectionAsync();
con.Notification += (_, e) => Console.WriteLine($"[{e.Channel}] {e.Payload}");

await using (var cmd = new NpgsqlCommand("LISTEN estoque_baixo", con))
    await cmd.ExecuteNonQueryAsync();

while (!ct.IsCancellationRequested)
    await con.WaitAsync(ct); // espera a próxima notificação

// Em outra conexão (ou num trigger): NOTIFY estoque_baixo, 'Produto 3 acabou';
```

**Comandos preparados.** Para SQL executado muitas vezes, o servidor pode guardar o plano de execução. Faça manualmente com `await cmd.PrepareAsync()` ou deixe automático com `Max Auto Prepare=20` na connection string.

**Outros tipos nativos úteis.**

| PostgreSQL | C# |
| --- | --- |
| `uuid` | `Guid` |
| `date` | `DateOnly` |
| `time` | `TimeOnly` |
| `interval` | `TimeSpan` |
| `inet` | `System.Net.IPAddress` |
| `bytea` | `byte[]` |
| `text[]`, `integer[]` | `string[]`, `int[]` |

## 16. Tratamento de erros

Todo erro vindo do banco herda de `DbException`. Capture o tipo específico do provedor quando precisar saber **qual** erro aconteceu (registro duplicado, chave estrangeira violada, banco fora do ar) e reagir de forma diferente para cada um.

**Hierarquia.**

| Exceção | Provedor | Informação útil |
| --- | --- | --- |
| `DbException` | Base de todos | `SqlState`, `IsTransient` (.NET 5+) |
| `SqliteException` | Microsoft.Data.Sqlite | `SqliteErrorCode`, `SqliteExtendedErrorCode` |
| `NpgsqlException` | Npgsql (erros de rede, timeout) | `IsTransient` |
| `PostgresException` | Npgsql (erro devolvido pelo servidor) | `SqlState`, `ConstraintName`, `TableName`, `Detail` |

**Exemplo: tratando nome duplicado nos dois bancos.** Suponha um índice único em `produtos(nome)`:

```csharp
using Microsoft.Data.Sqlite;
using Npgsql;

try
{
    repo.Inserir("Teclado", 150m, 10);
}
catch (PostgresException ex) when (ex.SqlState == PostgresErrorCodes.UniqueViolation) // "23505"
{
    Console.WriteLine($"Já existe um produto com esse nome (restrição {ex.ConstraintName}).");
}
catch (SqliteException ex) when (ex.SqliteErrorCode == 19) // SQLITE_CONSTRAINT
{
    Console.WriteLine($"Violação de restrição no SQLite: {ex.Message}");
}
catch (DbException ex) when (ex.IsTransient)
{
    Console.WriteLine("Falha temporária (rede, servidor ocupado). Tente de novo.");
}
catch (DbException ex)
{
    Console.WriteLine($"Erro de banco: {ex.Message}");
    throw;
}
```

**Códigos SQLSTATE mais comuns no PostgreSQL** (constantes em `PostgresErrorCodes`):

| Código | Constante | Significado |
| --- | --- | --- |
| `23505` | `UniqueViolation` | Valor duplicado em coluna única |
| `23503` | `ForeignKeyViolation` | Referência a um registro que não existe (ou exclusão de um registro referenciado) |
| `23502` | `NotNullViolation` | Faltou valor em coluna obrigatória |
| `40001` | `SerializationFailure` | Conflito entre transações; repita a operação |
| `40P01` | `DeadlockDetected` | Duas transações travaram uma à outra; repita |
| `42P01` | `UndefinedTable` | Tabela não existe |
| `28P01` | `InvalidPassword` | Usuário ou senha incorretos |

**Nova tentativa (retry) para falhas temporárias.** Quedas de rede rápidas e conflitos de serialização costumam se resolver tentando de novo. Um retry simples com espera crescente:

```csharp
static async Task<T> ComRetryAsync<T>(Func<Task<T>> operacao, int tentativas = 3)
{
    for (int i = 1; ; i++)
    {
        try
        {
            return await operacao();
        }
        catch (DbException ex) when (
            i < tentativas &&
            (ex.IsTransient || ex.SqlState is "40001" or "40P01"))
        {
            await Task.Delay(TimeSpan.FromMilliseconds(200 * Math.Pow(2, i))); // 400, 800 ms...
        }
    }
}

var lista = await ComRetryAsync(() => repo.ListarAsync());
```

Em projetos maiores, a biblioteca [Polly](https://github.com/App-vNext/Polly) oferece políticas de retry prontas e configuráveis.

**Boas práticas.**

- Não mostre `ex.Message` do banco para o usuário final: pode revelar nomes de tabelas e detalhes internos. Registre no log e mostre uma mensagem amigável.
- Só repita operações que podem ser repetidas com segurança. Uma transação inteira pode; um `INSERT` solto, fora de transação, pode acabar duplicado.
- Não engula exceções com `catch { }` vazio: o erro some e o problema aparece depois, longe da causa.

## 17. Organização do código em projetos reais

Em uma aplicação de verdade, o acesso a dados fica isolado em **repositórios**, a connection string vem da **configuração** e as dependências são entregues por **injeção de dependência** (DI). Assim o resto do sistema não sabe nem se importa com qual banco está por trás.

**1. Connection string no appsettings.json.**

```json
{
  "ConnectionStrings": {
    "Loja": "Host=localhost;Database=loja;Username=postgres;Password=senha123"
  }
}
```

Em desenvolvimento, tire a senha do arquivo e use o *User Secrets* (fica fora da pasta do projeto e não vai para o Git):

```bash
dotnet user-secrets init
dotnet user-secrets set "ConnectionStrings:Loja" "Host=localhost;Database=loja;Username=postgres;Password=senha123"
```

Em produção, use variáveis de ambiente (`ConnectionStrings__Loja=...`) ou um cofre de segredos do provedor de nuvem.

**2. Uma interface para o repositório.** O resto do código depende da interface, não da implementação:

```csharp
public interface IProdutoRepositorio
{
    Task<List<Produto>> ListarAsync(CancellationToken ct = default);
    Task<Produto?> ObterPorIdAsync(int id, CancellationToken ct = default);
    Task<int> InserirAsync(string nome, decimal preco, int estoque, CancellationToken ct = default);
    Task<bool> AtualizarAsync(Produto produto, CancellationToken ct = default);
    Task<bool> ExcluirAsync(int id, CancellationToken ct = default);
}
```

**3. Implementação recebendo o DbDataSource por DI.**

```csharp
public class ProdutoRepositorio(DbDataSource dataSource) : IProdutoRepositorio
{
    public async Task<Produto?> ObterPorIdAsync(int id, CancellationToken ct = default)
    {
        await using var cmd = dataSource.CreateCommand(
            "SELECT id, nome, preco, estoque, criado_em FROM produtos WHERE id = @id");
        var p = cmd.CreateParameter();
        p.ParameterName = "@id";
        p.Value = id;
        cmd.Parameters.Add(p);

        await using var r = await cmd.ExecuteReaderAsync(ct);
        return await r.ReadAsync(ct)
            ? new Produto(r.GetInt32(0), r.GetString(1), r.GetDecimal(2), r.GetInt32(3), r.GetDateTime(4))
            : null;
    }

    // ... demais métodos no mesmo estilo
}
```

**4. Registro no ASP.NET Core (Program.cs).** O pacote `Npgsql.DependencyInjection` registra a `NpgsqlDataSource` como singleton (uma única instância, com o pool, para a aplicação toda):

```bash
dotnet add package Npgsql.DependencyInjection
```

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNpgsqlDataSource(
    builder.Configuration.GetConnectionString("Loja")!);
builder.Services.AddScoped<IProdutoRepositorio, ProdutoRepositorio>();

var app = builder.Build();

app.MapGet("/produtos/{id:int}", async (int id, IProdutoRepositorio repo, CancellationToken ct) =>
    await repo.ObterPorIdAsync(id, ct) is { } produto
        ? Results.Ok(produto)
        : Results.NotFound());

app.Run();
```

Para SQLite, como não há `DbDataSource` pronto, registre uma fábrica de conexões:

```csharp
builder.Services.AddSingleton<Func<DbConnection>>(_ =>
    () => new SqliteConnection(builder.Configuration.GetConnectionString("Loja")));
```

**5. Versionando o esquema do banco (migrações).** Guarde os scripts SQL numerados no projeto (`001_criar_produtos.sql`, `002_criar_vendas.sql`) e uma tabela no banco que registra quais já rodaram. Ferramentas prontas que fazem isso com SQL puro: [DbUp](https://dbup.readthedocs.io/), [Evolve](https://evolve-db.netlify.app/) e [FluentMigrator](https://fluentmigrator.github.io/).

**6. Testando o acesso a dados.**

- **SQLite em memória**: rápido e sem instalação. Abra uma conexão `Data Source=:memory:`, crie as tabelas e mantenha a conexão aberta durante o teste.
- **PostgreSQL real com Testcontainers**: o pacote `Testcontainers.PostgreSql` sobe um PostgreSQL descartável no Docker para cada execução de testes. É a forma mais fiel, porque testa o SQL exato que vai para produção.
- Não use SQLite para testar código feito para PostgreSQL: diferenças de tipos e de SQL escondem erros.

## 18. Desempenho e boas práticas

A maior parte dos problemas de desempenho com banco de dados vem de poucos erros repetidos: consultas demais, dados demais e conexões mal gerenciadas. Esta lista resume o que verificar.

**Checklist.**

- [ ] Toda conexão, comando, reader e transação está dentro de `using` / `await using`.
- [ ] Conexões são abertas por operação e fechadas logo depois; nenhuma conexão fica guardada em campo estático.
- [ ] Todos os valores vão por parâmetro; nenhum SQL concatena entrada do usuário.
- [ ] Os `SELECT`s listam só as colunas necessárias, nunca `SELECT *` em código de produção.
- [ ] Listagens têm paginação (`LIMIT`/`OFFSET` ou paginação por chave).
- [ ] Colunas usadas em `WHERE`, `JOIN` e `ORDER BY` frequentes têm índice.
- [ ] Não há consultas dentro de laços (o problema N+1, abaixo).
- [ ] Inserções em massa usam transação (SQLite) ou `COPY`/batch (PostgreSQL).
- [ ] Em web e interface gráfica, só métodos `Async`, com `CancellationToken` repassado.
- [ ] Connection strings e senhas estão fora do código-fonte.

**O problema N+1.** Buscar uma lista e depois fazer uma consulta para cada item gera 1 + N idas ao banco:

```csharp
// LENTO: 1 consulta de vendas + 1 consulta de produto para CADA venda
foreach (var venda in vendas)
{
    var produto = await repo.ObterPorIdAsync(venda.ProdutoId);
}
```

```sql
-- RÁPIDO: uma consulta só, com JOIN
SELECT v.id, v.quantidade, p.nome, p.preco
  FROM vendas v
  JOIN produtos p ON p.id = v.produto_id
 WHERE v.vendido_em >= @desde;
```

**Paginação.** Com `OFFSET` grande o banco ainda precisa percorrer as linhas puladas. Para tabelas grandes, pagine pela chave ("keyset"): guarde o último id visto e peça os próximos.

```sql
-- Simples (bom para poucas páginas)
SELECT id, nome FROM produtos ORDER BY id LIMIT 20 OFFSET 40;

-- Por chave (rápido em qualquer página)
SELECT id, nome FROM produtos WHERE id > @ultimoId ORDER BY id LIMIT 20;
```

**Índices.**

```sql
CREATE INDEX IF NOT EXISTS ix_produtos_nome ON produtos (nome);
CREATE UNIQUE INDEX IF NOT EXISTS ux_produtos_nome ON produtos (nome); -- também impede duplicados
```

Para descobrir se uma consulta usa índice, rode `EXPLAIN QUERY PLAN SELECT ...` no SQLite ou `EXPLAIN ANALYZE SELECT ...` no PostgreSQL.

**Leitura sequencial de colunas grandes.** Para ler BLOBs ou textos muito grandes sem carregar tudo na memória, use `CommandBehavior.SequentialAccess` e leia as colunas na ordem, com `GetStream` ou `GetTextReader`:

```csharp
await using var r = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess);
while (await r.ReadAsync())
{
    await using var stream = r.GetStream(0);
    await stream.CopyToAsync(arquivoDestino);
}
```

**Medindo antes de otimizar.** Use `System.Diagnostics.Stopwatch` em volta da operação e o log de consultas lentas do PostgreSQL (`log_min_duration_statement`). O Npgsql também emite métricas e rastreamento via OpenTelemetry com o pacote `Npgsql.OpenTelemetry`.

## 19. ADO.NET vs Dapper vs Entity Framework Core

Os três não competem: Dapper e EF Core são construídos **em cima** do ADO.NET. A escolha é sobre quanto trabalho repetitivo você quer que a ferramenta faça por você, e quanto controle aceita abrir mão.

| Critério | ADO.NET puro | Dapper | EF Core |
| --- | --- | --- | --- |
| Quem escreve o SQL | Você | Você | A ferramenta (a partir de LINQ) |
| Mapeamento linha → objeto | Manual (`reader.GetX`) | Automático | Automático |
| Controle e transparência | Total | Quase total | Menor; o SQL gerado precisa ser conferido |
| Desempenho | Máximo | Muito próximo do ADO.NET puro | Bom; algum custo extra por recursos |
| Código repetitivo | Muito | Pouco | Mínimo |
| Migrações de esquema | Por conta própria | Por conta própria | Incluídas |
| Curva de aprendizado | SQL + API simples | SQL + API simples | Maior: rastreamento de mudanças, LINQ, configurações |
| Recursos específicos do banco (COPY, LISTEN) | Acesso direto | Acesso direto à conexão | Limitado; volta-se ao ADO.NET |

**A mesma consulta nos três.**

```csharp
// ADO.NET
await using var cmd = dataSource.CreateCommand("SELECT id, nome, preco, estoque, criado_em FROM produtos WHERE preco > @min");
cmd.Parameters.AddWithValue("min", 100m);
var lista = new List<Produto>();
await using var r = await cmd.ExecuteReaderAsync();
while (await r.ReadAsync())
    lista.Add(new Produto(r.GetInt32(0), r.GetString(1), r.GetDecimal(2), r.GetInt32(3), r.GetDateTime(4)));

// Dapper (pacote Dapper): mesma conexão ADO.NET, mapeamento automático
await using var con = await dataSource.OpenConnectionAsync();
var lista2 = await con.QueryAsync<Produto>(
    "SELECT id, nome, preco, estoque, criado_em AS CriadoEm FROM produtos WHERE preco > @min",
    new { min = 100m });

// EF Core: sem SQL, a partir de LINQ
var lista3 = await db.Produtos.Where(p => p.Preco > 100m).ToListAsync();
```

**Quando usar cada um.**

- **ADO.NET puro**: para aprender (este tutorial), bibliotecas e ferramentas de baixo nível, cargas em massa, recursos específicos do banco e trechos onde cada milissegundo importa.
- **Dapper**: quando você quer escrever o SQL, mas não quer escrever o mapeamento. Ótimo meio-termo para APIs e relatórios.
- **EF Core**: aplicações com muitas entidades e regras de negócio, em que produtividade e migrações automáticas valem mais que controle fino do SQL.

É comum combinar: EF Core para o CRUD do dia a dia e ADO.NET ou Dapper para relatórios pesados e importações. Como tudo usa a mesma `DbConnection` por baixo, os três convivem no mesmo projeto, e o que você aprendeu aqui (conexões, pool, parâmetros, transações) vale para todos.

## 20. Referência rápida, glossário e exercícios

**Modelo para copiar: operação completa e assíncrona.**

```csharp
await using var con = new NpgsqlConnection(cs);       // ou SqliteConnection
await con.OpenAsync(ct);
await using var tx = await con.BeginTransactionAsync(ct);   // se houver mais de um comando

await using var cmd = con.CreateCommand();
cmd.Transaction = tx;
cmd.CommandText = "UPDATE produtos SET preco = @preco WHERE id = @id";
cmd.Parameters.AddWithValue("@preco", 10m);
cmd.Parameters.AddWithValue("@id", 1);
int afetadas = await cmd.ExecuteNonQueryAsync(ct);

await tx.CommitAsync(ct);
```

**Qual método usar?**

| Quero... | Use |
| --- | --- |
| Inserir, alterar, excluir, criar tabela | `ExecuteNonQueryAsync` |
| Um único valor (contagem, soma, id novo) | `ExecuteScalarAsync` |
| Várias linhas | `ExecuteReaderAsync` + `ReadAsync` |
| Vários comandos tudo ou nada | `BeginTransactionAsync` + `CommitAsync` |
| Milhares de inserções no PostgreSQL | `BeginBinaryImportAsync` (COPY) |
| Milhares de inserções no SQLite | Transação + comando reaproveitado |
| Vários comandos numa ida ao servidor | `NpgsqlBatch` |
| Dados em memória para um grid | `DataTable.Load(reader)` |

**Erros comuns e soluções.**

| Mensagem ou sintoma | Causa provável | Solução |
| --- | --- | --- |
| `SQLite Error 1: 'no such table'` | Tabela não criada, ou o programa abriu outro arquivo `.db` (caminho relativo) | Rode o `CREATE TABLE`; use caminho absoluto para conferir |
| `database is locked` | Duas escritas simultâneas no SQLite | Ative WAL, encurte transações, feche conexões |
| `InvalidCastException` no `GetString` | Coluna com NULL ou tipo diferente | `IsDBNull` antes; confira a tabela de tipos (seção 7) |
| `Execute requires the command to have a transaction object` | Comando sem `Transaction` numa conexão com transação aberta (SQLite) | `cmd.Transaction = tx` |
| `The connection pool has been exhausted` | Conexões não fechadas | `using` em todas as conexões |
| `28P01: password authentication failed` | Usuário ou senha incorretos | Confira a connection string |
| `Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone'` | `DateTime.Now` enviado como `timestamptz` | Use `DateTime.UtcNow` |
| `There is already an open DataReader` | Novo comando antes de fechar o reader anterior na mesma conexão | Feche o reader (`using`) ou use outra conexão |

**Glossário.**

| Termo | Definição |
| --- | --- |
| ADO.NET | Conjunto de classes do .NET para acesso a bancos de dados |
| Provedor (driver) | Biblioteca que implementa o ADO.NET para um banco específico (Npgsql, Microsoft.Data.Sqlite) |
| Connection string | Texto com os dados para conectar ao banco |
| Pool de conexões | Reserva de conexões abertas reaproveitadas para economizar tempo |
| Parâmetro | Valor enviado separado do SQL, protegendo contra SQL Injection |
| SQL Injection | Ataque que insere comandos SQL através de dados digitados pelo usuário |
| DataReader | Leitor de resultados linha a linha, só para frente |
| Transação | Grupo de comandos executados no esquema tudo ou nada |
| Commit / Rollback | Confirmar / desfazer uma transação |
| Isolamento | Quanto uma transação enxerga das alterações de outras simultâneas |
| Índice | Estrutura que acelera buscas por determinadas colunas |
| N+1 | Erro de fazer uma consulta para cada item de uma lista |
| ORM | Ferramenta que mapeia tabelas para classes e gera SQL (ex.: EF Core) |
| Micro-ORM | ORM leve que só mapeia resultados; você escreve o SQL (ex.: Dapper) |
| UTC | Horário universal, sem fuso; o padrão para guardar datas |

**Exercícios propostos** (do mais fácil ao mais difícil).

- [ ] Crie o projeto da seção 3 e a tabela `produtos` no SQLite; insira três produtos e liste-os no console.
- [ ] Faça um menu no console (1-Listar, 2-Inserir, 3-Alterar preço, 4-Excluir) usando o `ProdutoRepositorio` da seção 9.
- [ ] Adicione uma busca por parte do nome com `LIKE` e parâmetro. Tente digitar `' OR '1'='1` e confirme que nada vaza.
- [ ] Converta todo o repositório para métodos `Async` com `CancellationToken`.
- [ ] Crie a tabela `vendas` e implemente `RegistrarVendaAsync` com transação. Teste vender mais que o estoque e confira que nada foi gravado.
- [ ] Rode o mesmo programa contra o PostgreSQL do Docker trocando apenas a fábrica de conexão.
- [ ] Importe 100 mil produtos de um arquivo CSV: no SQLite com transação, no PostgreSQL com `COPY`. Compare os tempos com `Stopwatch`.
- [ ] Trate o erro de nome duplicado (índice único) com mensagens amigáveis nos dois bancos.
- [ ] Crie uma API mínima em ASP.NET Core com `AddNpgsqlDataSource` e endpoints de CRUD.
- [ ] Escreva testes de integração usando Testcontainers com PostgreSQL.

**Para continuar estudando.**

- [Documentação do ADO.NET (Microsoft Learn)](https://learn.microsoft.com/dotnet/framework/data/adonet/)
- [Documentação do Microsoft.Data.Sqlite](https://learn.microsoft.com/dotnet/standard/data/sqlite/)
- [Documentação do Npgsql](https://www.npgsql.org/doc/)
- [Documentação do PostgreSQL](https://www.postgresql.org/docs/)
- [Documentação do SQLite](https://www.sqlite.org/docs.html)

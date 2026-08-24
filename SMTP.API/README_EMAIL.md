# 📧 Sistema de Envio de Emails - SMTP.API

## Descrição

Sistema completo de envio de emails desenvolvido com **.NET 10** e **C#** que integra perfeitamente com **Angular**. Inclui validação de dados, tratamento de erros robusto, injeção de dependência, logging detalhado e endpoints RESTful prontos para produção.

---

## 🏗️ Arquitetura

### Estrutura de Pastas
```
SMTP.API/
├── SendEmail/
│   ├── Email.cs                 # Implementação do serviço SMTP
│   ├── EmailRequest.cs          # Modelo de requisição
│   ├── EmailResponse.cs         # Modelo de resposta
│   └── IEmailService.cs         # Interface do serviço
├── Controllers/
│   └── EmailController.cs       # API REST para email
├── appsettings.json             # Configurações SMTP
├── Program.cs                   # Configuração DI e CORS
└── SMTP.API.http               # Exemplos de requisições
```

---

## 🔧 Configuração

### 1. Credenciais SMTP (appsettings.json)

```json
{
  "SMTP": {
	"Host": "smtp.gmail.com",
	"Port": "587",
	"Username": "seu-email@gmail.com",
	"Password": "sua-senha-ou-app-password",
	"EmailOrigem": "seu-email@gmail.com",
	"EnableSSL": true
  }
}
```

**Para Gmail:**
- Habilite "Acesso a apps menos seguros" ou
- Use uma **Senha de App** (Google Account → Security → App passwords)

---

## 📚 Endpoints da API

### 1️⃣ Health Check
**Verificar se o serviço está rodando**

```
GET /api/email/health
```

**Resposta (200 OK):**
```json
{
  "status": "Email service is running",
  "timestamp": "2026-08-20T18:37:09.999257Z",
  "environment": "Development"
}
```

---

### 2️⃣ Enviar Email
**Enviar um email para um ou mais destinatários**

```
POST /api/email/enviar
Content-Type: application/json
```

**Body:**
```json
{
  "destinatarios": ["email1@example.com", "email2@example.com"],
  "assunto": "Assunto do Email",
  "corpo": "<h1>Título</h1><p>Conteúdo em HTML</p>",
  "anexos": []
}
```

**Resposta (200 OK - Sucesso):**
```json
{
  "sucesso": true,
  "mensagem": "Email enviado com sucesso!",
  "dataEnvio": "2026-08-20T18:40:15.123456Z"
}
```

**Resposta (400 Bad Request - Erro):**
```json
{
  "sucesso": false,
  "mensagem": "Deve haver pelo menos um destinatário.",
  "dataEnvio": "2026-08-20T18:40:15.123456Z"
}
```

---

### 3️⃣ Validar Email
**Validar se um endereço de email é válido**

```
POST /api/email/validar
Content-Type: application/json
```

**Body:**
```json
"email@example.com"
```

**Resposta (200 OK):**
```json
{
  "valido": true,
  "mensagem": "Email válido"
}
```

---

## 🛡️ Validações Implementadas

| Validação | Descrição |
|-----------|-----------|
| **Destinatários vazios** | Pelo menos um email é obrigatório |
| **Assunto vazio** | Assunto não pode estar em branco |
| **Corpo vazio** | Corpo do email é obrigatório |
| **Email inválido** | Formato de email validado com `MailAddress` |
| **Arquivo não encontrado** | Verifica existência de anexos antes de enviar |
| **Credenciais SMTP** | Valida conexão com servidor de email |

---

## 🔐 Tratamento de Erros

```csharp
try
{
	// 1. Validação de entrada (ArgumentException)
	// 2. Conexão SMTP (SmtpException)
	// 3. Erros inesperados (Exception genérica)
}
catch (ArgumentException ex)  // Erro de validação
catch (SmtpException ex)       // Erro SMTP
catch (Exception ex)           // Qualquer outro erro
```

**Todas as exceções são logadas** e retornam uma resposta amigável ao cliente.

---

## 📝 Exemplos de Uso

### C# / .NET

```csharp
using HttpClient client = new();
var request = new
{
	destinatarios = new[] { "usuario@example.com" },
	assunto = "Bem-vindo!",
	corpo = "<h1>Olá!</h1>",
	anexos = new string[] { }
};

var json = JsonSerializer.Serialize(request);
var content = new StringContent(json, Encoding.UTF8, "application/json");
var response = await client.PostAsync("http://localhost:5062/api/email/enviar", content);

if (response.IsSuccessStatusCode)
{
	var result = await response.Content.ReadAsStringAsync();
	Console.WriteLine(result); // Email enviado!
}
```

---

### Angular/TypeScript

```typescript
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class EmailService {
  private apiUrl = 'http://localhost:5062/api/email';

  constructor(private http: HttpClient) { }

  enviarEmail(request: EmailRequest): Observable<EmailResponse> {
	return this.http.post<EmailResponse>(`${this.apiUrl}/enviar`, request);
  }

  validarEmail(email: string): Observable<{ valido: boolean; mensagem: string }> {
	return this.http.post<any>(`${this.apiUrl}/validar`, email);
  }
}
```

---

### Angular Component

```typescript
export class FormularioEmailComponent {
  formulario = this.fb.group({
	destinatarios: ['', Validators.required],
	assunto: ['', Validators.required],
	corpo: ['', Validators.required]
  });

  constructor(
	private emailService: EmailService,
	private fb: FormBuilder
  ) { }

  enviarEmail() {
	if (!this.formulario.valid) return;

	const request: EmailRequest = {
	  destinatarios: this.formulario.value.destinatarios.split(',').map(e => e.trim()),
	  assunto: this.formulario.value.assunto,
	  corpo: this.formulario.value.corpo,
	  anexos: []
	};

	this.emailService.enviarEmail(request).subscribe(
	  (response) => {
		if (response.sucesso) {
		  alert('Email enviado com sucesso!');
		  this.formulario.reset();
		} else {
		  alert('Erro: ' + response.mensagem);
		}
	  },
	  (error) => {
		alert('Erro ao enviar email: ' + error.message);
	  }
	);
  }
}
```

---

## 🚀 Executando a Aplicação

### 1. Restaurar pacotes
```bash
dotnet restore
```

### 2. Executar migrations (se necessário)
```bash
dotnet ef database update
```

### 3. Rodar a aplicação
```bash
dotnet run
```

A aplicação estará disponível em:
- **HTTP**: http://localhost:5062
- **Swagger UI**: http://localhost:5062/swagger/index.html
- **Health Check**: http://localhost:5062/api/email/health

---

## 🧪 Testando com Swagger UI

1. Acesse: `http://localhost:5062/swagger/index.html`
2. Encontre a seção **Email**
3. Clique em **POST /api/email/enviar**
4. Clique em **Try it out**
5. Preencha o JSON e clique em **Execute**

---

## 🧪 Testando com Rest Client (VS Code)

Abra o arquivo `SMTP.API.http` no Visual Studio e clique em "Send Request" acima de cada teste.

---

## 🔄 CORS Configuration

A aplicação está configurada para aceitar requisições de **qualquer origem**:

```csharp
builder.Services.AddCors(options =>
{
	options.AddPolicy("LiberarTudo", policy =>
	{
		policy.AllowAnyOrigin()        // Qualquer origem
			  .AllowAnyMethod()        // Qualquer método (GET, POST, etc)
			  .AllowAnyHeader();       // Qualquer header
	});
});
```

---

## 📊 Logging

Todas as operações são logadas:

```
info: SMTP.API.SendEmail.Email[0]
	  Iniciando processo de envio de email

info: SMTP.API.SendEmail.Email[0]
	  Validação da requisição concluída com sucesso

info: SMTP.API.SendEmail.Email[0]
	  Conectando ao servidor SMTP: smtp.gmail.com:587

info: SMTP.API.SendEmail.Email[0]
	  Email enviado com sucesso pelo SMTP
```

---

## ⚠️ Segurança - Boas Práticas

### ❌ NÃO FAÇA EM PRODUÇÃO:
- ❌ Colocar credenciais no `appsettings.json`
- ❌ Usar `AllowAnyOrigin()` sem restrição
- ❌ Desabilitar HTTPS

### ✅ FAÇA EM PRODUÇÃO:
- ✅ Use **Azure Key Vault** ou **AWS Secrets Manager**
- ✅ Configure CORS com origens específicas
- ✅ Force HTTPS sempre
- ✅ Implemente autenticação/autorização
- ✅ Use variáveis de ambiente

**Exemplo com User Secrets (Desenvolvimento):**
```bash
dotnet user-secrets set "SMTP:Password" "sua-senha-real"
```

---

## 🐛 Troubleshooting

### Erro: "Credenciais SMTP inválidas"
- ✅ Verifique username e password no `appsettings.json`
- ✅ Para Gmail, use uma **Senha de App** (não a senha normal)
- ✅ Habilite "Acesso a apps menos seguros" na conta Google

### Erro: "Servidor SMTP não encontrado"
- ✅ Verifique a URL do host SMTP (smtp.gmail.com)
- ✅ Confirme que a porta está correta (587 para TLS, 465 para SSL)
- ✅ Teste conexão com `Test-NetConnection`

### Erro 400 "Email inválido"
- ✅ Verifique o formato do email (user@domain.com)
- ✅ Certifique-se de que não há espaços em branco
- ✅ Use o endpoint `/api/email/validar` para testar

---

## 📦 Dependências

```xml
<PackageReference Include="Microsoft.AspNetCore.OpenApi" Version="10.0.10" />
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="10.0.11" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="10.2.3" />
<PackageReference Include="MediatR" Version="14.2.0" />
```

---

## 📄 Versão

- **.NET**: 10.0
- **Linguagem**: C# 13
- **Framework**: ASP.NET Core
- **Data**: Agosto de 2026

---

## 👨‍💻 Desenvolvido por

Especialista Full Stack C#/.NET/Angular com mais de 10 anos de experiência

---

## 📞 Suporte

Para dúvidas ou problemas:
1. Verifique os logs da aplicação
2. Consulte o arquivo `SMTP.API.http` para exemplos
3. Teste o endpoint `/api/email/health`
4. Verifique as configurações SMTP

---

**🎉 Sistema pronto para integração com Angular!**

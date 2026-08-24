//// ============================================================================
//// 📧 MODELOS TYPESCRIPT PARA ANGULAR - Sistema de Email
//// ============================================================================
//// Copie este arquivo para seu projeto Angular
//// Caminho recomendado: src/app/models/email.models.ts
//// ============================================================================

///**
// * Modelo de requisição para envio de email
// */
//export interface EmailRequest {
//  /** Lista de emails destinatários */
//  destinatarios: string[];

//  /** Assunto do email */
//  assunto: string;

//  /** Corpo do email (suporta HTML) */
//  corpo: string;

//  /** Lista opcional de caminhos de anexos */
//  anexos?: string[];
//}

///**
// * Modelo de resposta após envio de email
// */
//export interface EmailResponse {
//  /** Indica se o email foi enviado com sucesso */
//  sucesso: boolean;

//  /** Mensagem de retorno (sucesso ou erro) */
//  mensagem: string;

//  /** Data e hora do envio em UTC */
//  dataEnvio: string;
//}

///**
// * Modelo de resposta do health check
// */
//export interface EmailHealthResponse {
//  /** Status do serviço */
//  status: string;

//  /** Timestamp do health check */
//  timestamp: string;

//  /** Ambiente de execução */
//  environment: string;
//}

///**
// * Modelo de resposta da validação de email
// */
//export interface EmailValidationResponse {
//  /** Indica se o email é válido */
//  valido: boolean;

//  /** Mensagem de validação */
//  mensagem: string;
//}

//// ============================================================================
//// 📧 SERVIÇO ANGULAR
//// ============================================================================
//// Caminho recomendado: src/app/services/email.service.ts
//// ============================================================================

//import { Injectable } from '@angular/core';
//import { HttpClient } from '@angular/common/http';
//import { Observable } from 'rxjs';

//@Injectable({
//  providedIn: 'root'
//})
//export class EmailService {
//  private apiUrl = 'http://localhost:5062/api/email';

//  constructor(private http: HttpClient) { }

//  /**
//   * Envia um email
//   * @param request Dados do email
//   * @returns Observable com resposta do servidor
//   */
//  enviarEmail(request: EmailRequest): Observable<EmailResponse> {
//    return this.http.post<EmailResponse>(`${this.apiUrl}/enviar`, request);
//  }

//  /**
//   * Valida um endereço de email
//   * @param email Email a validar
//   * @returns Observable com resultado da validação
//   */
//  validarEmail(email: string): Observable<EmailValidationResponse> {
//    return this.http.post<EmailValidationResponse>(`${this.apiUrl}/validar`, `"${email}"`);
//  }

//  /**
//   * Verifica se o serviço de email está rodando
//   * @returns Observable com status do serviço
//   */
//  health(): Observable<EmailHealthResponse> {
//    return this.http.get<EmailHealthResponse>(`${this.apiUrl}/health`);
//  }
//}

//// ============================================================================
//// ✉️ COMPONENTE ANGULAR DE EXEMPLO
//// ============================================================================
//// Caminho recomendado: src/app/components/formulario-email/formulario-email.component.ts
//// ============================================================================

//import { Component, OnInit } from '@angular/core';
//import { FormBuilder, FormGroup, Validators } from '@angular/forms';

//@Component({
//  selector: 'app-formulario-email',
//  template: `
//    <div class="container mt-5">
//      <div class="row">
//        <div class="col-md-8 mx-auto">
//          <h1>📧 Enviar Email</h1>

//          <!-- Alerta de Sucesso -->
//          <div *ngIf="mensagemSucesso" class="alert alert-success alert-dismissible fade show" role="alert">
//            {{ mensagemSucesso }}
//            <button type="button" class="btn-close" (click)="mensagemSucesso = ''"></button>
//          </div>

//          <!-- Alerta de Erro -->
//          <div *ngIf="mensagemErro" class="alert alert-danger alert-dismissible fade show" role="alert">
//            {{ mensagemErro }}
//            <button type="button" class="btn-close" (click)="mensagemErro = ''"></button>
//          </div>

//          <!-- Formulário -->
//          <form [formGroup]="formulario" (ngSubmit)="onSubmit()">
//            <!-- Campo: Destinatários -->
//            <div class="mb-3">
//              <label for="destinatarios" class="form-label">
//                Destinatários <span class="text-danger">*</span>
//              </label>
//              <input
//                type="email"
//                class="form-control"
//                id="destinatarios"
//                formControlName="destinatarios"
//                placeholder="email1@example.com, email2@example.com"
//                [class.is-invalid]="
//                  formulario.get('destinatarios')?.invalid &&
//                  formulario.get('destinatarios')?.touched
//                "
//              />
//              <div class="invalid-feedback">
//                <span *ngIf="formulario.get('destinatarios')?.hasError('required')">
//                  Destinatários são obrigatórios
//                </span>
//              </div>
//            </div>

//            <!-- Campo: Assunto -->
//            <div class="mb-3">
//              <label for="assunto" class="form-label">
//                Assunto <span class="text-danger">*</span>
//              </label>
//              <input
//                type="text"
//                class="form-control"
//                id="assunto"
//                formControlName="assunto"
//                placeholder="Digite o assunto do email"
//                [class.is-invalid]="
//                  formulario.get('assunto')?.invalid &&
//                  formulario.get('assunto')?.touched
//                "
//              />
//              <div class="invalid-feedback">
//                <span *ngIf="formulario.get('assunto')?.hasError('required')">
//                  Assunto é obrigatório
//                </span>
//              </div>
//            </div>

//            <!-- Campo: Corpo -->
//            <div class="mb-3">
//              <label for="corpo" class="form-label">
//                Corpo do Email <span class="text-danger">*</span>
//              </label>
//              <textarea
//                class="form-control"
//                id="corpo"
//                formControlName="corpo"
//                rows="6"
//                placeholder="Digite o conteúdo do email (suporta HTML)"
//                [class.is-invalid]="
//                  formulario.get('corpo')?.invalid &&
//                  formulario.get('corpo')?.touched
//                "
//              ></textarea>
//              <small class="text-muted">Dica: Você pode usar HTML (ex: <h1>Título</h1>)</small>
//              <div class="invalid-feedback">
//                <span *ngIf="formulario.get('corpo')?.hasError('required')">
//                  Corpo é obrigatório
//                </span>
//              </div>
//            </div>

//            <!-- Botões -->
//            <div class="d-flex gap-2">
//              <button
//                type="submit"
//                class="btn btn-primary"
//                [disabled]="enviando || formulario.invalid"
//              >
//                <span *ngIf="!enviando">✉️ Enviar Email</span>
//                <span *ngIf="enviando">
//                  <span class="spinner-border spinner-border-sm me-2" role="status" aria-hidden="true"></span>
//                  Enviando...
//                </span>
//              </button>
//              <button
//                type="button"
//                class="btn btn-secondary"
//                (click)="onReset()"
//                [disabled]="enviando"
//              >
//                🔄 Limpar
//              </button>
//            </div>
//          </form>
//        </div>
//      </div>
//    </div>
//  `,
//  styles: []
//})
//export class FormularioEmailComponent implements OnInit {
//  formulario!: FormGroup;
//  enviando = false;
//  mensagemSucesso = '';
//  mensagemErro = '';

//  constructor(
//    private fb: FormBuilder,
//    private emailService: EmailService
//  ) {}

//  ngOnInit(): void {
//    this.inicializarFormulario();
//    this.verificarSaude();
//  }

//  /**
//   * Inicializa o formulário com validações
//   */
//  private inicializarFormulario(): void {
//    this.formulario = this.fb.group({
//      destinatarios: ['', [Validators.required, Validators.minLength(5)]],
//      assunto: ['', [Validators.required, Validators.minLength(5)]],
//      corpo: ['', [Validators.required, Validators.minLength(10)]]
//    });
//  }

//  /**
//   * Verifica se o serviço de email está funcionando
//   */
//  private verificarSaude(): void {
//    this.emailService.health().subscribe(
//      (response) => {
//        console.log('✅ Serviço de email está funcionando', response);
//      },
//      (error) => {
//        console.error('❌ Erro ao conectar com serviço de email', error);
//        this.mensagemErro = 'Serviço de email indisponível. Tente novamente mais tarde.';
//      }
//    );
//  }

//  /**
//   * Submete o formulário de envio de email
//   */
//  onSubmit(): void {
//    if (this.formulario.invalid) {
//      alert('Por favor, preencha todos os campos corretamente.');
//      return;
//    }

//    this.enviando = true;
//    this.mensagemSucesso = '';
//    this.mensagemErro = '';

//    // Parsear destinatários (separados por vírgula)
//    const destinatarios = this.formulario.value.destinatarios
//      .split(',')
//      .map((email: string) => email.trim());

//    const request: EmailRequest = {
//      destinatarios: destinatarios,
//      assunto: this.formulario.value.assunto,
//      corpo: this.formulario.value.corpo,
//      anexos: []
//    };

//    this.emailService.enviarEmail(request).subscribe(
//      (response) => {
//        this.enviando = false;

//        if (response.sucesso) {
//          this.mensagemSucesso = '✅ ' + response.mensagem;
//          this.formulario.reset();
//        } else {
//          this.mensagemErro = '❌ ' + response.mensagem;
//        }
//      },
//      (error) => {
//        this.enviando = false;
//        this.mensagemErro = '❌ Erro ao enviar email: ' + error.error?.mensagem || error.message;
//        console.error('Erro detalhado:', error);
//      }
//    );
//  }

//  /**
//   * Reseta o formulário
//   */
//  onReset(): void {
//    this.formulario.reset();
//    this.mensagemSucesso = '';
//    this.mensagemErro = '';
//  }
//}

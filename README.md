# Entrega API

## Overview

A API Entrega fornece endpoints para gerenciar entregas, incluindo criação, atualizações de status e recuperação de informações de entrega.

---

## Instalação e Execuçao

1. **Requisitos:**
   - [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

2. **Configuração:**
   - **Clone o repositório**:
    ```bash
    git clone https://github.com/seu-usuario/transporte.git
    cd transporte
    ```
   - Run the API:
   ```sh
   dotnet run
   ```
   - A API estará disponível em `https://localhost:7002/` ou `http://localhost:5280` por padrão.

---

## Endpoints

### Criar Entrega

- **URL:** `POST /api/entrega`
- **Descrição:** Cria uma nova entrega.
- **Request Body:**
  ```json
  { "cliente": "string", "endereco": "string", "produto": "string", "dataEstimadaEntrega": "2025-08-24T00:00:00", "observacoes": "string (optional)" }
  ```
- **Response:** 200.

---

### Obter entregas (com Paginação)

- **URL:** `GET /api/entrega`
- **Descrição:** Recupera uma lista paginada de entregas, opcionalmente filtrada por status ou código/cliente.
- **Query Params:**
  - `page` (int, obrigatório): Número da página.
  - `pageSize` (int, obrigatório): Itens por página.
  - `clienteCodigo` (string, opcional): Filtra por código ou nome do cliente.
  - `status` (int, opcional): Filtra por status de entrega (consulte a enumeração `StatusEntrega`).
- **Response:**
  ```json
  { "items": [ /* array de EntregaDto */ ], "totalCount": 100 }
  ```
---

### Obter entrega por ID

- **URL:** `GET /api/entrega/{id}`
- **Descrição:** Recupera uma entrega por seu ID exclusivo.
- **Response:** `200 OK` com detalhes de entrega, ou `404 Não Encontrado` se não for encontrado.

---

### Atualizar status de entrega

- **URL:** `PUT /api/entrega/{id}/status`
- **Descrição:** Atualiza o status de uma entrega.
- **Request Body:**
  ```json
  { "status": 2 // (int) Corresponde a StatusEntrega enum }
  ```
- **Response:** `200 OK` com entrega atualizada, ou `400 Bad Request` se inválido.

---

## StatusEntrega Enum

| Value | Name            |
|-------|-----------------|
| 0     | PENDENTE        |
| 1     | EM_ROTA         |
| 2     | ENTREGUE        |
| 3     | CANCELADA       |
| 4     | PEDIDO_CRIADO |
| 5     | SAIU_ENTREGA    |
| 6     | COLETADO        |

## Data Models

### EntregaDto

- `id` (string)
- `cliente` (string)
- `dataEnvio` (DateTime)
- `endereco` (string)
- `produto` (string)
- `dataEstimadaEntrega` (DateTime)
- `observacoes` (string, optional)
- `posts` (array of HistoricoEntregaDto)

- ### HistoricoEntregaDto

- `id` (string)
- `entregaId` (string)
- `status` (int)
- `data` (DateTime)

## Error Handling

- `400 Bad Request`: Invalid input or business rule violation.
- `404 Not Found`: Resource not found.
- `500 Internal Server Error`: Unexpected error.

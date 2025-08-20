Parte 1 – Parte1Controller (Número Aleatório Único)

Requisitos:
Cada chamada deve retornar um número diferente.
O número salvo no banco deve ser único, evitando exceções de duplicidade.

O que foi feito:
O RandomService injeta via DI, garantindo que a geração de números não dependa de uma instância fixa de Random e
salva no banco apenas números únicos (checando duplicidade antes de inserir).
A funcionalidade de gerar número aleatório único está implementada e injetada via DI.

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Parte 2 – Parte2Controller (Paginação de Produtos)
Requisitos:
Paginação correta dos produtos (10 por página, resultados diferentes conforme page).
Usar injeção de dependência para ProductService.
Reduzir repetição de código nos modelos CustomerList e ProductList.
Melhorar CustomerService e ProductService para evitar repetição.

O que foi feito:
Foi criado a paginação usando Skip e Take no ProductService.
Houve uma alteração do controller para usar DI ao invés de instanciar o serviço manualmente.
Foi criado um padrão mais genérico para retornar listas paginadas (evitando duplicação de código entre CustomerList e ProductList).
A paginação funciona corretamente, DI está sendo usado, e a repetição de código foi reduzida.

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Parte 3 – Parte3Controller (Pagamento e Orders)
Requisitos:
Diversas formas de pagamento devem ser suportadas sem alterar OrderService para cada nova forma (Open-Closed Principle).
OrderDate deve ser salvo em UTC, mas retornado em fuso horário brasileiro (UTC-3).

O que foi feito:
Foi implementado o IPaymentStrategy e as classes PixPayment, CreditCardPayment, PaypalPayment.
O OrderService usa uma estratégia de pagamento (_paymentStrategies) em vez de if/else.
Controllers ajustados para converter OrderDate de UTC → Horário de Brasília antes de retornar.
Adicionado DI para injeção de estratégias de pagamento.

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Parte 4 – Parte4Controller (Validação CanPurchase)

Requisitos:
Criar testes unitários cobrindo todas as regras de negócio do método CanPurchase.
Possível re-arquitetura para facilitar testes.

O que foi feito:
Criada a classe CustomerServiceTests com cobertura ampla:
CustomerId inválido
Cliente não encontrado
Cliente já comprou no mês
Compra acima do limite no primeiro pedido
Fora do horário comercial

Testes usam InMemoryDatabase, Moq para IDateTimeProvider, e cobrem todas as regras do CanPurchase.

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
[Parte1Controller] 
     |
     | GET /random
     v
[RandomService]
     |
     | Gera número aleatório
     v
[TestDbContext.Orders] 
     |
     | Salva número no banco
     v
[Retorna número aleatório único]
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
[Parte2Controller] 
     |
     | GET /products?page={n}
     v
[ProductService] 
     |
     | Query:
     |   - Skip((page-1)*10)
     |   - Take(10)
     v
[ProductListModel] / [CustomerListModel] (reduz repetição)
     |
     v
[Retorna 10 produtos da página solicitada]

---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
[Parte3Controller]
     |
     | POST /pay ou GET /orders
     v
[OrderService] 
     |
     | Busca strategy correspondente:
     |   - PixPayment
     |   - CreditCardPayment
     |   - PaypalPayment
     v
[IPaymentStrategy.ProcessPayment]
     |
     | Executa lógica específica do método de pagamento
     v
[TestDbContext.Orders] 
     |
     | Salva pedido (OrderDate em UTC)
     v
[Order retornado]
     |
     | Converte OrderDate para horário brasileiro (UTC-3)
     v
[Resposta Controller]


---------------------------------------------------------------------------------------------------------------------------------------------------------------------------
[Parte4Controller]
     |
     | GET /canpurchase?customerId=1&value=10
     v
[CustomerService] (injetado via DI)
     |
     | Regras de negócio aplicadas:
     |   - customerId inválido
     |   - cliente não encontrado
     |   - já comprou este mês
     |   - primeira compra acima de 100
     |   - fora do horário comercial
     v
[TestDbContext.Customers + Orders] 
     |
     v
[Retorna TRUE/FALSE] 
     |
     v
[Testes unitários CustomerServiceTests]

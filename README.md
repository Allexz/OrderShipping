# SPRINT: UDEMY COURSES
 ### **Iniciado em: 13/06** 

## Comparação entre Tipos de Exchanges

| Tipo       | Routing Key         | Performance      | Complexidade     | Caso Típico de Uso          |
|------------|--------------------|------------------|------------------|-----------------------------|
| Direct     | Exata (correspondência perfeita) | Alta        | Baixa        | Comandos, RPC               |
| Fanout     | Ignorada           | Muito Alta       | Mínima           | Broadcast/Notificações      |
| Topic      | Padrão (com wildcards `*` e `#`) | Média       | Média-Alta    | Eventos Complexos           |
| Headers    | Não usa (somente headers) | Baixa | Alta         | Roteamento por Metadados    |
 
 ## Branch master
 ### Implementação básica de uma API(Minimal API) fazendo requisições a um ENDPOINT do tipo RABBITMQ FANOUT sobre WEBAPI/C#
#
 ## Branch feature/direct-exchange
 ### Evolução do projeto para utilização de EXCHANGE do tipo DIRECT RABBITMQ sobre WEBAPI/C#  
 Uma característica do deste tipo de EXCHANGE, __DIRECT__, é a existência de ROUTING KEYS, que são parâmetros adicionados às mensagens que direcionam, por correspondência exata, às QUEUEUS configuradas para atender àquele parâmetro. 

 #
 ## Branch feature/topic-exchange
 ### Evolução do projeto para utilização de EXCHANGE do tipo TOPIC RABBITMQ sobre WEBAPI/C#  
 Já a característica deste tipo de EXCHANGE, __TOPIC__, é a utilização de _WILDCARDS_ para direcionamento das mensagens.    
 1. order.* - As QUEUES receberão mensagens que contenham uma única palavra após o padrão _order._ (order.product, order.stock, order.stop)
 2. order.# - As QUEUES receberão mensagens que contenham qualquer número de palavras após o padrão _order.#_ (order.payment.rejected, order.payment.accepted, order.payment.unauthorized)

#
### Notas:

1. __cfg.ExchangeType = "fanout"__;
   Define o tipo padrão de EXCHANGE para todas EXCHANGES criadas pelo MASSTRANSIT. É uma configuração global que afeta toda a topologia.
   Usar em cenários simples onde todas as mensagens usam o mesmo tipo de EXCHANGE.

2. __cfg.Publish<OrderPlacedMessage>(x => x.ExchangeType = "fanout")__;
   Define o tipo de EXCHANGE específico para a mensagem tipo ORDERPLACEDMESSAGE. Permite a configuração granular por tipo de mensagem.
   Utilize em cenários complexos com múltiplos tipos de roteamento  .
   
3. __Binding__ (vinculação) é a configuração que define como mensagens são roteadas de uma exchange (no RabbitMQ) ou um tópico (em outros brokers) para uma fila específica. Ele estabelece as regras de filtragem e entrega de mensagens com base em routing keys ou padrões de assinatura.
  




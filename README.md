# SPRINT: UDEMY COURSES
 ### **Iniciado em: 13/06** 
 ## Branch master
 ### Implementação básica de uma API(Minimal API) fazendo requisições a um ENDPOINT do tipo RABBITMQ sobre WEBAPI/C#
#
 ## Branch feature/direct-exchange
 ### Evolução do projeto para utilização de EXCHANGE do tipo DIRECT RABBITMQ sobre WEBAPI/C#

#
 Notas:
### Diferenças entre os dois comandos:
1. cfg.ExchangeType = "fanout";
2. cfg.Publish<OrderPlacedMessage>(x => x.ExchangeType = "fanout");
- O primeiro define o tipo padrão de EXCHANGE para todas EXCHANGES criadas pelo MASSTRANSIT.  
É uma configuração global que afeta toda a topologia.
Usar em cenários simples onde todas as mensagens usam o mesmo tipo de EXCHANGE
- O segundo define o tipo de EXCHANGE específico para a mensagem tipo ORDERPLACEDMESSAGE.  
  Permite a configuração granular por tipo de mensagem.  
  Utilize em cenários complexos com múltiplos tipos de roteamento  .
  #




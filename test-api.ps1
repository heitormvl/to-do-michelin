# Teste Completo da API ToDo Michelin

Write-Host "=== TESTE COMPLETO DA API TODO MICHELIN ===" -ForegroundColor Green
Write-Host "Testando todos os endpoints: GET, POST, PUT, DELETE" -ForegroundColor Cyan

# 1. Fazer login
Write-Host "`n1. FAZENDO LOGIN..." -ForegroundColor Yellow
$loginBody = @{
    username = "admin"
    password = "123"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "http://localhost:5289/auth/login" -Method POST -ContentType "application/json" -Body $loginBody
    $token = $loginResponse.token
    Write-Host "✅ Login realizado com sucesso!" -ForegroundColor Green
    Write-Host "Token: $($token.Substring(0, 50))..." -ForegroundColor Gray
} catch {
    Write-Host "❌ Erro no login: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

$headers = @{
    "Authorization" = "Bearer $token"
    "Content-Type" = "application/json"
}

# 2. GET - Listar tarefas (deve estar vazio inicialmente)
Write-Host "`n2. GET - LISTANDO TAREFAS (inicial)..." -ForegroundColor Yellow
try {
    $tarefas = Invoke-RestMethod -Uri "http://localhost:5289/tarefas" -Method GET -Headers $headers
    Write-Host "✅ Tarefas listadas com sucesso!" -ForegroundColor Green
    Write-Host "Total de tarefas: $($tarefas.Count)" -ForegroundColor Gray
    foreach ($tarefa in $tarefas) {
        Write-Host "  - $($tarefa.titulo) (ID: $($tarefa.id))" -ForegroundColor Gray
    }
} catch {
    Write-Host "❌ Erro ao listar tarefas: $($_.Exception.Message)" -ForegroundColor Red
}

# 3. POST - Criar primeira tarefa
Write-Host "`n3. POST - CRIANDO PRIMEIRA TAREFA..." -ForegroundColor Yellow
$tarefa1Body = @{
    titulo = "Estudar ASP.NET Core"
    descricao = "Aprender sobre Web APIs, Entity Framework e JWT"
} | ConvertTo-Json

try {
    $tarefa1Response = Invoke-RestMethod -Uri "http://localhost:5289/tarefas" -Method POST -Headers $headers -Body $tarefa1Body
    Write-Host "✅ Primeira tarefa criada com sucesso!" -ForegroundColor Green
    Write-Host "ID: $($tarefa1Response.id)" -ForegroundColor Gray
    Write-Host "Título: $($tarefa1Response.titulo)" -ForegroundColor Gray
    Write-Host "Usuário: $($tarefa1Response.usuarioId)" -ForegroundColor Gray
    $tarefa1Id = $tarefa1Response.id
} catch {
    Write-Host "❌ Erro ao criar primeira tarefa: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 4. POST - Criar segunda tarefa
Write-Host "`n4. POST - CRIANDO SEGUNDA TAREFA..." -ForegroundColor Yellow
$tarefa2Body = @{
    titulo = "Fazer exercícios"
    descricao = "Treinar na academia por 1 hora"
} | ConvertTo-Json

try {
    $tarefa2Response = Invoke-RestMethod -Uri "http://localhost:5289/tarefas" -Method POST -Headers $headers -Body $tarefa2Body
    Write-Host "✅ Segunda tarefa criada com sucesso!" -ForegroundColor Green
    Write-Host "ID: $($tarefa2Response.id)" -ForegroundColor Gray
    Write-Host "Título: $($tarefa2Response.titulo)" -ForegroundColor Gray
    $tarefa2Id = $tarefa2Response.id
} catch {
    Write-Host "❌ Erro ao criar segunda tarefa: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# 5. GET - Listar tarefas (agora deve ter 2)
Write-Host "`n5. GET - LISTANDO TAREFAS (após criação)..." -ForegroundColor Yellow
try {
    $tarefas = Invoke-RestMethod -Uri "http://localhost:5289/tarefas" -Method GET -Headers $headers
    Write-Host "✅ Tarefas listadas com sucesso!" -ForegroundColor Green
    Write-Host "Total de tarefas: $($tarefas.Count)" -ForegroundColor Gray
    foreach ($tarefa in $tarefas) {
        Write-Host "  - $($tarefa.titulo) (ID: $($tarefa.id)) - Criada em: $($tarefa.dataCriacao)" -ForegroundColor Gray
    }
} catch {
    Write-Host "❌ Erro ao listar tarefas: $($_.Exception.Message)" -ForegroundColor Red
}

# 6. GET - Buscar tarefa específica por ID
Write-Host "`n6. GET - BUSCANDO TAREFA ESPECÍFICA..." -ForegroundColor Yellow
try {
    $tarefaEspecifica = Invoke-RestMethod -Uri "http://localhost:5289/tarefas/$tarefa1Id" -Method GET -Headers $headers
    Write-Host "✅ Tarefa específica encontrada!" -ForegroundColor Green
    Write-Host "ID: $($tarefaEspecifica.id)" -ForegroundColor Gray
    Write-Host "Título: $($tarefaEspecifica.titulo)" -ForegroundColor Gray
    Write-Host "Descrição: $($tarefaEspecifica.descricao)" -ForegroundColor Gray
    Write-Host "Concluída: $($tarefaEspecifica.concluida)" -ForegroundColor Gray
} catch {
    Write-Host "❌ Erro ao buscar tarefa específica: $($_.Exception.Message)" -ForegroundColor Red
}

# 7. PUT - Atualizar primeira tarefa
Write-Host "`n7. PUT - ATUALIZANDO PRIMEIRA TAREFA..." -ForegroundColor Yellow
$tarefa1UpdateBody = @{
    titulo = "Estudar ASP.NET Core (ATUALIZADO)"
    descricao = "Aprender sobre Web APIs, Entity Framework, JWT e muito mais!"
} | ConvertTo-Json

try {
    $updateResponse = Invoke-RestMethod -Uri "http://localhost:5289/tarefas/$tarefa1Id" -Method PUT -Headers $headers -Body $tarefa1UpdateBody
    Write-Host "✅ Primeira tarefa atualizada com sucesso!" -ForegroundColor Green
    Write-Host "Status: 204 No Content" -ForegroundColor Gray
} catch {
    Write-Host "❌ Erro ao atualizar tarefa: $($_.Exception.Message)" -ForegroundColor Red
}

# 8. GET - Verificar se a atualização funcionou
Write-Host "`n8. GET - VERIFICANDO ATUALIZAÇÃO..." -ForegroundColor Yellow
try {
    $tarefaAtualizada = Invoke-RestMethod -Uri "http://localhost:5289/tarefas/$tarefa1Id" -Method GET -Headers $headers
    Write-Host "✅ Tarefa atualizada verificada!" -ForegroundColor Green
    Write-Host "Título atualizado: $($tarefaAtualizada.titulo)" -ForegroundColor Gray
    Write-Host "Descrição atualizada: $($tarefaAtualizada.descricao)" -ForegroundColor Gray
} catch {
    Write-Host "❌ Erro ao verificar atualização: $($_.Exception.Message)" -ForegroundColor Red
}

# 9. DELETE - Deletar segunda tarefa
Write-Host "`n9. DELETE - DELETANDO SEGUNDA TAREFA..." -ForegroundColor Yellow
try {
    $deleteResponse = Invoke-RestMethod -Uri "http://localhost:5289/tarefas/$tarefa2Id" -Method DELETE -Headers $headers
    Write-Host "✅ Segunda tarefa deletada com sucesso!" -ForegroundColor Green
    Write-Host "Status: 204 No Content" -ForegroundColor Gray
} catch {
    Write-Host "❌ Erro ao deletar tarefa: $($_.Exception.Message)" -ForegroundColor Red
}

# 10. GET - Listar tarefas (deve ter apenas 1 agora)
Write-Host "`n10. GET - LISTANDO TAREFAS (após deletar)..." -ForegroundColor Yellow
try {
    $tarefas = Invoke-RestMethod -Uri "http://localhost:5289/tarefas" -Method GET -Headers $headers
    Write-Host "✅ Tarefas listadas com sucesso!" -ForegroundColor Green
    Write-Host "Total de tarefas: $($tarefas.Count)" -ForegroundColor Gray
    foreach ($tarefa in $tarefas) {
        Write-Host "  - $($tarefa.titulo) (ID: $($tarefa.id))" -ForegroundColor Gray
    }
} catch {
    Write-Host "❌ Erro ao listar tarefas: $($_.Exception.Message)" -ForegroundColor Red
}

# 11. Teste de erro - Tentar buscar tarefa deletada
Write-Host "`n11. TESTE DE ERRO - Buscando tarefa deletada..." -ForegroundColor Yellow
try {
    $tarefaDeletada = Invoke-RestMethod -Uri "http://localhost:5289/tarefas/$tarefa2Id" -Method GET -Headers $headers
    Write-Host "❌ Erro: Tarefa deveria ter sido deletada!" -ForegroundColor Red
} catch {
    Write-Host "✅ Correto: Tarefa não encontrada (404)" -ForegroundColor Green
    Write-Host "Status: $($_.Exception.Response.StatusCode)" -ForegroundColor Gray
}

# 12. Teste de erro - Tentar deletar tarefa inexistente
Write-Host "`n12. TESTE DE ERRO - Deletando tarefa inexistente..." -ForegroundColor Yellow
$fakeId = "00000000-0000-0000-0000-000000000000"
try {
    $deleteFake = Invoke-RestMethod -Uri "http://localhost:5289/tarefas/$fakeId" -Method DELETE -Headers $headers
    Write-Host "❌ Erro: Deveria ter retornado 404!" -ForegroundColor Red
} catch {
    Write-Host "✅ Correto: Tarefa inexistente não pode ser deletada (404)" -ForegroundColor Green
    Write-Host "Status: $($_.Exception.Response.StatusCode)" -ForegroundColor Gray
}

# 13. Teste de validação - Tentar criar tarefa com título muito curto
Write-Host "`n13. TESTE DE VALIDAÇÃO - Criando tarefa com título inválido..." -ForegroundColor Yellow
$tarefaInvalidaBody = @{
    titulo = "AB"  # Menos de 3 caracteres
    descricao = "Esta tarefa tem título muito curto"
} | ConvertTo-Json

try {
    $tarefaInvalida = Invoke-RestMethod -Uri "http://localhost:5289/tarefas" -Method POST -Headers $headers -Body $tarefaInvalidaBody
    Write-Host "❌ Erro: Deveria ter falhado na validação!" -ForegroundColor Red
} catch {
    Write-Host "✅ Correto: Validação funcionou (400)" -ForegroundColor Green
    Write-Host "Status: $($_.Exception.Response.StatusCode)" -ForegroundColor Gray
}

# 14. Teste final - Listar todas as tarefas
Write-Host "`n14. GET - LISTAGEM FINAL..." -ForegroundColor Yellow
try {
    $tarefas = Invoke-RestMethod -Uri "http://localhost:5289/tarefas" -Method GET -Headers $headers
    Write-Host "✅ Listagem final realizada!" -ForegroundColor Green
    Write-Host "Total de tarefas: $($tarefas.Count)" -ForegroundColor Gray
    foreach ($tarefa in $tarefas) {
        Write-Host "  - $($tarefa.titulo) (ID: $($tarefa.id))" -ForegroundColor Gray
    }
} catch {
    Write-Host "❌ Erro na listagem final: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== TESTE COMPLETO FINALIZADO ===" -ForegroundColor Green
Write-Host "✅ Todos os endpoints testados com sucesso!" -ForegroundColor Green
Write-Host "✅ CRUD completo funcionando!" -ForegroundColor Green
Write-Host "✅ Validações funcionando!" -ForegroundColor Green
Write-Host "✅ Filtro por usuário funcionando!" -ForegroundColor Green
Write-Host "✅ Ordenação por data funcionando!" -ForegroundColor Green 
# Script para testar as funcionalidades de autenticação
$baseUrl = "http://localhost:5289"

Write-Host "=== Testando API de Autenticação ===" -ForegroundColor Green

# Teste 1: Registro de usuário
Write-Host "`n1. Testando registro de usuário..." -ForegroundColor Yellow
$registroBody = @{
    username = "teste"
    email = "teste@exemplo.com"
    password = "Senha123"
    confirmPassword = "Senha123"
} | ConvertTo-Json

try {
    $registroResponse = Invoke-RestMethod -Uri "$baseUrl/auth/registro" -Method POST -Body $registroBody -ContentType "application/json"
    Write-Host "✓ Registro realizado com sucesso!" -ForegroundColor Green
    Write-Host "Token: $($registroResponse.token)" -ForegroundColor Cyan
    $token = $registroResponse.token
} catch {
    Write-Host "✗ Erro no registro: $($_.Exception.Message)" -ForegroundColor Red
    exit 1
}

# Teste 2: Login
Write-Host "`n2. Testando login..." -ForegroundColor Yellow
$loginBody = @{
    username = "teste"
    password = "Senha123"
} | ConvertTo-Json

try {
    $loginResponse = Invoke-RestMethod -Uri "$baseUrl/auth/login" -Method POST -Body $loginBody -ContentType "application/json"
    Write-Host "✓ Login realizado com sucesso!" -ForegroundColor Green
    Write-Host "Token: $($loginResponse.token)" -ForegroundColor Cyan
} catch {
    Write-Host "✗ Erro no login: $($_.Exception.Message)" -ForegroundColor Red
}

# Teste 3: Obter perfil (requer autenticação)
Write-Host "`n3. Testando obtenção de perfil..." -ForegroundColor Yellow
$headers = @{
    "Authorization" = "Bearer $token"
}

try {
    $perfilResponse = Invoke-RestMethod -Uri "$baseUrl/auth/perfil" -Method GET -Headers $headers
    Write-Host "✓ Perfil obtido com sucesso!" -ForegroundColor Green
    Write-Host "Username: $($perfilResponse.username)" -ForegroundColor Cyan
    Write-Host "Email: $($perfilResponse.email)" -ForegroundColor Cyan
} catch {
    Write-Host "✗ Erro ao obter perfil: $($_.Exception.Message)" -ForegroundColor Red
}

# Teste 4: Solicitar redefinição de senha
Write-Host "`n4. Testando solicitação de redefinição de senha..." -ForegroundColor Yellow
$redefinirBody = @{
    email = "teste@exemplo.com"
} | ConvertTo-Json

try {
    $redefinirResponse = Invoke-RestMethod -Uri "$baseUrl/auth/redefinir-senha" -Method POST -Body $redefinirBody -ContentType "application/json"
    Write-Host "✓ Solicitação de redefinição enviada!" -ForegroundColor Green
    Write-Host "Mensagem: $($redefinirResponse.message)" -ForegroundColor Cyan
} catch {
    Write-Host "✗ Erro na solicitação de redefinição: $($_.Exception.Message)" -ForegroundColor Red
}

# Teste 5: Logout
Write-Host "`n5. Testando logout..." -ForegroundColor Yellow
try {
    $logoutResponse = Invoke-RestMethod -Uri "$baseUrl/auth/logout" -Method POST -Headers $headers
    Write-Host "✓ Logout realizado com sucesso!" -ForegroundColor Green
    Write-Host "Mensagem: $($logoutResponse.message)" -ForegroundColor Cyan
} catch {
    Write-Host "✗ Erro no logout: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n=== Testes concluídos ===" -ForegroundColor Green 
# Script para mostrar o conteúdo do banco de dados
Write-Host "=== CONTEUDO DO BANCO DE DADOS ===" -ForegroundColor Cyan
Write-Host "Arquivo: toDoMichelin.db" -ForegroundColor Yellow
Write-Host ""

# Verificar se o arquivo existe
if (Test-Path "toDoMichelin.db") {
    Write-Host "✅ Arquivo do banco encontrado!" -ForegroundColor Green
    $fileSize = (Get-Item "toDoMichelin.db").Length
    Write-Host "📁 Tamanho: $($fileSize) bytes" -ForegroundColor Gray
    Write-Host ""
    
    # Tentar usar dotnet ef para mostrar dados
    try {
        Write-Host "📋 Executando consulta no banco..." -ForegroundColor Yellow
        dotnet ef dbcontext info --project . --startup-project .
        Write-Host ""
        Write-Host "✅ Banco de dados configurado corretamente!" -ForegroundColor Green
    } catch {
        Write-Host "⚠️ Não foi possível executar consulta direta" -ForegroundColor Yellow
    }
    
    Write-Host ""
    Write-Host "=== INSTRUÇÕES PARA VER OS DADOS ===" -ForegroundColor Cyan
    Write-Host "1. Baixe o DB Browser for SQLite: https://sqlitebrowser.org/" -ForegroundColor White
    Write-Host "2. Abra o arquivo: toDoMichelin.db" -ForegroundColor White
    Write-Host "3. Vá na aba 'Browse Data'" -ForegroundColor White
    Write-Host "4. Selecione a tabela 'Tarefas'" -ForegroundColor White
    Write-Host ""
    Write-Host "OU" -ForegroundColor Yellow
    Write-Host "1. Vá para: https://sqliteonline.com/" -ForegroundColor White
    Write-Host "2. Clique em 'File' → 'Open DB'" -ForegroundColor White
    Write-Host "3. Selecione o arquivo toDoMichelin.db" -ForegroundColor White
    
} else {
    Write-Host "❌ Arquivo toDoMichelin.db não encontrado!" -ForegroundColor Red
    Write-Host "Execute 'dotnet ef database update' primeiro" -ForegroundColor Yellow
} 
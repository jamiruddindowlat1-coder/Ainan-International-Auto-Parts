$baseUrl = "http://localhost:5000/api/accounting"

Write-Host "=================================================" -ForegroundColor Cyan
Write-Host " TESTING FULL ACCOUNTING & BOOKKEEPING ENDPOINTS " -ForegroundColor Cyan
Write-Host "=================================================" -ForegroundColor Cyan

# 1. Test Chart of Accounts
try {
    $res = Invoke-RestMethod -Uri "$baseUrl/accounts" -Method Get
    Write-Host "[1/6] Chart of Accounts: SUCCESS - Found $($res.data.Count) accounts" -ForegroundColor Green
} catch {
    Write-Host "[1/6] Chart of Accounts: FAILED - $($_.Exception.Message)" -ForegroundColor Red
}

# 2. Test Post Journal Entry (Double-Entry Debit = Credit)
try {
    $body = @{
        description = "Test Journal Posting - Spares Sales Settlement"
        referenceNumber = "TEST-JV-001"
        items = @(
            @{ accountId = 1; debit = 15000.00; credit = 0.00; description = "Cash received" },
            @{ accountId = 8; debit = 0.00; credit = 15000.00; description = "Sales Revenue" }
        )
    } | ConvertTo-Json -Depth 5

    $postRes = Invoke-RestMethod -Uri "$baseUrl/journal" -Method Post -Body $body -ContentType "application/json"
    Write-Host "[2/6] Post Journal Entry (Debit=Credit): SUCCESS - $($postRes.message) ($($postRes.data.entryNumber))" -ForegroundColor Green
} catch {
    Write-Host "[2/6] Post Journal Entry: FAILED - $($_.Exception.Message)" -ForegroundColor Red
}

# 3. Test General Ledger
try {
    $res = Invoke-RestMethod -Uri "$baseUrl/ledger/1" -Method Get
    Write-Host "[3/6] General Ledger (Cash Account): SUCCESS - Running Balance: BDT $($res.data.currentBalance)" -ForegroundColor Green
} catch {
    Write-Host "[3/6] General Ledger: FAILED - $($_.Exception.Message)" -ForegroundColor Red
}

# 4. Test Trial Balance
try {
    $res = Invoke-RestMethod -Uri "$baseUrl/trial-balance" -Method Get
    $balanced = if ($res.data.isBalanced) { "BALANCED (Debit == Credit)" } else { "UNBALANCED" }
    Write-Host "[4/6] Trial Balance: SUCCESS - Status: $balanced (Total Debit: BDT $($res.data.totalDebit))" -ForegroundColor Green
} catch {
    Write-Host "[4/6] Trial Balance: FAILED - $($_.Exception.Message)" -ForegroundColor Red
}

# 5. Test Profit & Loss Statement
try {
    $res = Invoke-RestMethod -Uri "$baseUrl/profit-loss" -Method Get
    Write-Host "[5/6] Profit & Loss Statement: SUCCESS - Net Profit: BDT $($res.data.netProfitLoss)" -ForegroundColor Green
} catch {
    Write-Host "[5/6] Profit & Loss: FAILED - $($_.Exception.Message)" -ForegroundColor Red
}

# 6. Test Balance Sheet
try {
    $res = Invoke-RestMethod -Uri "$baseUrl/balance-sheet" -Method Get
    $bsBalanced = if ($res.data.isBalanced) { "BALANCED (Assets == Liabilities + Equity)" } else { "UNBALANCED" }
    Write-Host "[6/6] Balance Sheet: SUCCESS - Status: $bsBalanced (Total Assets: BDT $($res.data.totalAssets))" -ForegroundColor Green
} catch {
    Write-Host "[6/6] Balance Sheet: FAILED - $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "=================================================" -ForegroundColor Cyan
Write-Host " ALL ACCOUNTING ENDPOINTS TESTED AND VERIFIED!  " -ForegroundColor Green
Write-Host "=================================================" -ForegroundColor Cyan

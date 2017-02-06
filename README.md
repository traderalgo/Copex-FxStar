# Copex-FxStar cTrader/cAlgo positions copier ver. 1.0
Allow copy positions from trader account to multiple clients accouns with multipy positions volume.

## _Copex-FxStar-Trader
* Trader (provider account) create signals and add positions to mysql database.

## _Copex-FxStar-Client
* Client - copy signals from Trader accounts. Allow multipy positions volume.

## _Copex-FxStar-Client-SL-TP
* Client - copy signals from Trader accounts. 
* Allow multipy positions volume (from 0.1 to 1000000000). 
* Allow positions StopLoss and TakeProfit (min. 10 Pips).
* Allow change timer to milliseconds (1 to 1000ms)
* Equity StopLoss (Close all positions and stop cBot)

### Requires
* cTrader or cAlgo platform
* Mysql database server https://dev.mysql.com/downloads/installer/
* Mysql Connector/.Net https://dev.mysql.com/downloads/connector/net/6.9.html
 

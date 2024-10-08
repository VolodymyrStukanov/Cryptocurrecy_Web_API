To run the application execute the commands in the cmd:
1) git clone https://github.com/VolodymyrStukanov/Cryptocurrecy_Web_API.git
2) cd Cryptocurrecy_Web_API
3) docker-compose up

API endpoints:
1) http://localhost:8088/api/Cryptocurrency/GetAvailableCryptocurrencies - returns all available currencies
2) http://localhost:8088/api/Cryptocurrency/GetCryptocurrencyPrice/:CurrencyAssetId - returns the currency rate

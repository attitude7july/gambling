# Gambling in which a random number between 0 - 9 is to be generated.
• The player has a starting account of 10,000 points and can use it to create any Set the partial amount to the randomly generated number. 
• If he is right, he gets 9 times his bet as a win
## Input contract
### The player sends his bet as a request to the program
Example:
<code>
{
 "points": 100
 "number": 3
}
</code>
## Output contract
### In the case of a successful bet, the player will receive his account balance back.
Example:
<code>
{
 "account": 10900
 "status": "won"
 "points": "+900"
}
</code>

#Running app
##First set your database or use in-memory database
##Set JwtSettings in appsettings for access_token
<code>
"JwtAppSettings": {
    "Secret": "",
    "ValidAudience": "",
    "ValidIssuer": "",
    "SessionMinutes": 20
  },
</code>
## Register user with email
## Login user with credentials
## Use access_token to access bet controller
## Place bet
## View betting history

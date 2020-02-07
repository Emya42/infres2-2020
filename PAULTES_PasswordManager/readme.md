# PAULTES Projet !

## Getting started 

Le front-end et le back-end sont exporté sous deux format **Linux-x64** et **Win-x64**.
La base de données est une base de données SQL SERVER ! Pour la migrer vers du MySQL ->
* [Migrer SQL SERVER DB vers MySQL](https://www.thegeekstuff.com/2014/03/mssql-to-mysql/)

## PREREQUIS !
 
Vous devez changer une ligne dans un fichier, ce fichier ce nomme **appsettings.json** .
La ligne est la suivante :
```
"DefaultConnection": "Data Source=************;Initial Catalog=master;Integrated Security=True;Persist Security Info=False",
```

Sur SQL Server vous pouvez récuperer votre **connexion string** directement à la connexion à celle-ci.

## Une fois tout ceci fait !

Si vous êtes sur Linux : 
* Executer **BackEndPassManag**
* Executer **FrontEndPassManager**

Si vous êtes sur Windows : 
* Executer **BackEndPassManag.exe**
* Executer **FrontEndPassManager.exe**

L'adresse de BackEnd est : https://localhost:5001/
L'adresse du FrontEnd est : https://localhost:5000/

**Merci !!**

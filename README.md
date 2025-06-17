## Projekt bemutatása
Ez a projekt egy ASP.NET Core Web API, amely egy tesztfeladat keretében készült, de jól szolgálhat más hasonló projekt alapjaként.
A szolgáltatás fő célja a munkanapok és munkaszüneti napok kezelése, a munkaszüneti napokat egy külső JSON fájlból menedzselve.
Funkcióit egy egyszerű, jQuery AJAX-ot használó HTML oldal teszteli.

## Főbb jellemzők
- Dátum Analízis: Egy adott dátumról megállapítja, hogy az munkanap vagy munkaszüneti nap.
- Intervallum Számítás: Két dátum között megszámolja a munkanapok és a munkaszüneti napok számát.
- Dinamikus Adatkezelés: Lehetőség van egy nap típusának módosítására (munkanapból munkaszüneti nap és fordítva), az eredményt a háttérben lévő JSON fájlba mentve.
- Külső Adatforrás: A munkaszüneti napok listája egy JSON fájlból van beolvasva és oda is van szerializálva.
- Naplózás: Részletes NLog naplózás minden metódus be- és kilépési pontján, a paraméterekkel és a felmerülő hibákkal együtt.
- Dátum Validáció: A rendszer 2016.01.01-től az aktuális év + 5 évig terjedő dátumokat fogad el.
- Teszt Frontend: A projekthez tartozik egy egyszerű HTML oldal, amely jQuery segítségével hívja meg és teszteli az API végpontjait.

## Használt technológiák
- Backend: ASP.NET Core Web API
- Nyelv: C#
- Naplózás: NLog
- Frontend: HTML, jQuery 

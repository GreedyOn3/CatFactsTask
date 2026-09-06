# CatFactsTask

Zadanie rekrutacyjne na rzecz Netwise Sp. z o.o.

Zadanie wykonałem w postaci aplikacji webowej ASP.NET Core. Napisałem ją głównie w .NET, bez wykorzystania JavaScriptu.

Wykorzystuje ona 3 zewnętrzne API:

- `https://catfact.ninja/fact` – wymagane w zadaniu API, z którego pobierane są fakty o kotach.
- `https://cataas.com/cat?json=true` – API CATAAS, z którego pobierane są informacje o losowym zdjęciu kota.
- `https://api.mymemory.translated.net/get?q={tekst}&langpair=en|pl` – API MyMemory wykorzystywane do tłumaczenia faktów z języka angielskiego na polski (tłumaczenie nie jest zbyt dokładne 😅).

Wszystkie powyższe API są wywoływane po stronie aplikacji w C#.

Aplikacja posiada również kilka dodatkowych funkcjonalności, takich jak:
- historia pobranych faktów,
- dodawanie faktów do ulubionych,
- oznaczanie faktów jako nielubianych,
- pomijanie nielubianych faktów podczas losowania kolejnych,
- Fakt Dnia,
- tłumaczenie faktów na język polski,
- wyszukiwanie faktów w historii, ulubionych i nielubianych.

## Uruchomienie aplikacji

W celu uruchomienia gotowej aplikacji należy wejść do folderu `app`, uruchomić plik `.exe`, a następnie wejść w przeglądarce na:

`https://localhost:5000`

Drugą możliwością jest pobranie rozwiązania, otwarcie go w Visual Studio i samodzielne zbudowanie oraz uruchomienie aplikacji.

## Zapisywanie danych

Aplikacja tworzy folder `Data` przy zapisie pierwszego faktu.

Jeżeli aplikacja jest uruchamiana z gotowego pliku `.exe` znajdującego się w folderze `app`, folder `Data` będzie widoczny bezpośrednio w tym folderze po zapisaniu pierwszego faktu.

Natomiast jeżeli aplikacja jest uruchamiana przez Visual Studio, folder `Data` znajduje się w:

`NetwiseTask\bin\Debug\net10.0`

W folderze są odpowiednio tworzone i znajdują się:

- `catfacts.txt` – plik `.txt` zapisujący wszystkie odpowiedzi otrzymane z endpointu `catfact.ninja/fact`, zgodnie z wymaganiami zadania.

- `favorites.txt` – plik przechowujący fakty o kotach dodane przez użytkownika do ulubionych.

- `banned.txt` – plik przechowujący nielubiane przez użytkownika fakty. Dzięki temu oznaczone fakty nie są ponownie losowane.

- `dailyfact.txt` – plik przechowujący aktualny Fakt Dnia. Dzięki temu w ciągu jednego dnia użytkownik otrzymuje ten sam fakt oraz zdjęcie. Nowy Fakt Dnia jest generowany dopiero po zmianie daty.


## Screenshoty aplikacji

### Strona główna

<img width="1920" height="1040" alt="Strona główna" src="https://github.com/user-attachments/assets/d03702ce-d3d3-4cd6-8017-d1eb60e4c2cc" />

### Menu

<img width="1920" height="1038" alt="Menu" src="https://github.com/user-attachments/assets/a010d6aa-4c5b-4625-9cc3-ea86d63cb878" />

### Losowe fakty

<img width="1919" height="1038" alt="Losowy fakt" src="https://github.com/user-attachments/assets/af940f53-246d-4716-91c6-307fe5f35578" />

<img width="1920" height="1039" alt="obraz" src="https://github.com/user-attachments/assets/cadf351d-9866-494e-a86a-1b5250c5a264" />


### Historia

<img width="1920" height="1042" alt="Historia" src="https://github.com/user-attachments/assets/79ebedaf-b385-4cef-9793-4ae93733d603" />




  

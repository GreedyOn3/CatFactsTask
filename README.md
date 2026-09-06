# CatFactsTask

Zadanie rekrutacyjne na rzecz Netwise S.A.

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

`http://localhost:5000`

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

<img width="1920" height="1040" alt="obraz" src="https://github.com/user-attachments/assets/b74c5577-124f-4bdf-b321-9fa264ea30e6" />


### Menu

<img width="1920" height="1037" alt="obraz" src="https://github.com/user-attachments/assets/98ba3346-ce59-4a1b-ae10-e497c6783562" />


### Losowe fakty

<img width="1918" height="1033" alt="obraz" src="https://github.com/user-attachments/assets/a18b6e42-9d34-4544-9313-2adb194c503e" />

<img width="1919" height="1036" alt="obraz" src="https://github.com/user-attachments/assets/1af3233e-1e66-4f37-8e9c-461fe26f5841" />

### Historia

<img width="1920" height="1036" alt="obraz" src="https://github.com/user-attachments/assets/a2ff7902-3738-4469-a37d-290591c7af84" />





  

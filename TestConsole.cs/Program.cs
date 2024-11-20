using System.Net.Http.Json;
using TestConsole.cs;

Console.WriteLine("Hello, World!");

while (true){
    Console.WriteLine("Выберите под каким пользователем зайти: ");
    Console.WriteLine("Неавторизованный пользователь: 1");
    Console.WriteLine("Бухгалтер: 2");
    Console.WriteLine("Маркетолог: 3");
    Console.WriteLine("Admin: 4");
    string selected = Console.ReadLine();

    if(!int.TryParse(selected, out int id) || id < 1 || id > 4)
        continue;

    string login = "";
    string password = "";

    switch(id){
        case 1:
            login = "user";
            password = "12345";
            break;
        case 2:
            login = "buchgalter";
            password = "12345";
            break;
        case 3:
            login = "marketolog";
            password = "12345";
            break;
        case 4:
            login = "admin";
            password = "12345";
            break;
    }

    ResponseTokenAndRole tokenAndRole = await Client.HttpClient.GetFromJsonAsync<ResponseTokenAndRole>($"login?login={login}&password={password}");
    Console.WriteLine($"Token: {tokenAndRole.Token}");  
    Console.WriteLine($"Role: {tokenAndRole.Role}");  

    Client.SetToken(tokenAndRole.Token);

    try {
        BaseResponce message = await Client.HttpClient.GetFromJsonAsync<BaseResponce>("data");
        Console.WriteLine("Общедоступное: " + message.Message);
    } catch (Exception ex){
        Console.WriteLine("Общедоступное: " + "Недостаточно прав доступа!");
    }
    try {
        BaseResponce message = await Client.HttpClient.GetFromJsonAsync<BaseResponce>("data/buch");
        Console.WriteLine("Бухгалтер: " + message.Message);
    } catch (Exception ex){
        Console.WriteLine("Бухгалтер: " + "Недостаточно прав доступа!");
    }
    try {
        BaseResponce message = await Client.HttpClient.GetFromJsonAsync<BaseResponce>("data/market");
        Console.WriteLine("Маркетолог: " + message.Message);
    } catch (Exception ex){
        Console.WriteLine("Маркетолог: " + "Недостаточно прав доступа!");
    }
    try {
        BaseResponce message = await Client.HttpClient.GetFromJsonAsync<BaseResponce>("data/admin");
        Console.WriteLine("Админ: " + message.Message);
    } catch (Exception ex){
        Console.WriteLine("Админ: " + "Недостаточно прав доступа!");
    }
    
    Console.WriteLine("------------------------------------");
}

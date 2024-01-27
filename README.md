![ER MiniProjekt](https://github.com/NoaDenise/MiniProjekt/assets/146171194/108e5ed2-8215-4572-b335-8fe4ee0fc0f4)
![UML Miniprojekt1](https://github.com/NoaDenise/MiniProjekt/assets/146171194/3c3129d2-289c-47f0-be18-f45940d8703b)

## Endpoints examples:

```json
Endpoint 1 (GET) - View all persons in the system
URL: `https://localhost:7174/viewpersons`

[
    {
        "id": 1,
        "firstName": "Denise",
        "lastName": "Is",
        "phoneNumber": "0705594848"
    },
    {
        "id": 2,
        "firstName": "John",
        "lastName": "Doe",
        "phoneNumber": "0734460000"
    },
    {
        "id": 3,
        "firstName": "Jane",
        "lastName": "Wahl",
        "phoneNumber": "0784457799"
    }
]

Endpoint 2 (GET) - View all interests linked to a person
URL: https://localhost:7174/viewInterest/2

{
    "contentType": null,
    "serializerSettings": null,
    "statusCode": null,
    "value": [
        {
            "personId": 2,
            "firstName": "John",
            "lastName": "Doe",
            "interestId": 1,
            "interestTitle": "Hiking",
            "interestDescription": "Walking for long distances"
        }
    ]
}
Endpoint 3 (GET) - View URLs connected to a person
URL: https://localhost:7174/viewUrl/2

[
    {
        "personId": 2,
        "interestId": 1,
        "url": "https://visitsweden.com/what-to-do/nature-outdoors/hiking/top-hiking-routes-sweden/"
    }
]

Endpoint 4 (POST) - Connect a person to a new interest
URL: https://localhost:7174/connectPersonToInterest/3/1
No JSON body needed for this request.

Endpoint 5 (POST) - Add a new URL for a person and interest
URL: https://localhost:7174/addLink
Request Body:

{
    "PersonId": 3,
    "InterestId": 4,
    "Url": "https://www.udemy.com/topic/baking/"
}

Response:
{
    "id": 5,
    "personId": 3,
    "interestId": 4,
    "url": "https://www.udemy.com/topic/baking/"
}


 

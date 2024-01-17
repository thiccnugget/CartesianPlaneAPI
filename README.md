
  # CartesianPlaneAPI  
  CartesianPlaneAPI is a REST API which lets users interact with a cartesian plane
  using simple HTTP requests.

  
  ## Features
  This API operates on a set of points distributed across the plane.  
  Currently, it exposes 4 endpoints:  
   - [POST]   /point
   - [GET]    /space
   - [GET]    /lines/:n
   - [DELETE] /space

    
   ### Endpoints Description

  - [POST] /point  
  This endpoint requires a body with 2 numeric properties: "x" and "y".  
  Adds a new point to the space with the given <b>x</b> and <b>y</b> coordinates.  
  The response gives status code <b>201</b> if the point has been created succesfully, 
  else <b>401</b> if the body or any of it's properties is incorrect.  
  The response also contains a message "msg" which gives information on the event.
    
  <b>Body Example</b>  
  ```json
  {  
    "x": 10,  
    "y": 20  
  }   
  ``` 
 
###

  - [GET] /space  
  This returns a JSON array of points sorted on the x axis.  
  Does not require params nor query params.  
  The response contains two properties:
  - msg: gives the number of points in the space;
  - data: an array of points.  

  <b>Response Example</b>  
  ```json
  {  
    "msg": "There are 3 points in this space",  
    "data":[  
      {
        "x": 1,
        "y": 1
      },
      {
        "x": 2,
        "y": 2
      },
      {
        "x": 3,
        "y": 3
      }      
    ]  
  }
  ```

###

  - [GET] /lines/:n  
  This returns a JSON array of segments, each defined by a set of points.  
  Requires a numeric param (n) which represents the minimum number of point each segment must pass through.  
  The response contains two properties:
  - msg: specifies the number of points inside each segment;
  - data: an array of segments.  

  <b>Response Example</b>  
  ```json
  {  
    "msg": "Lines passing through 3 points.",  
    "data":[
      [  
        {
          "x": 1,
          "y": 1
        },
        {
          "x": 2,
          "y": 2
        },
        {
          "x": 3,
          "y": 3
        }
      ],  
      [  
        {
          "x": 0,
          "y": 1
        },
        {
          "x": 0,
          "y": 2
        },
        {
          "x": 0,
          "y": 3
        }
      ] 
      ...
    ]  
  }
  ```
  <b>NOTE:</b>
  The :n param must be a number equal or greater than 2, else an error response will be returned.

###
  - [DELETE] /space  
  This endpoint does not require any parameter and deletes all points from the space. Only returns a confirmation message.
  
  <b>Response Example</b>  
  ```json
  {  
    "msg": "All points in space deleted" 
  }   
  ``` 


## Development 
This server is developed in C# .NET 7 using the WebAPI configuration.  
Each endpoint is clustered under a specific route (/), inside a single controller.  
Points are saved locally in a SortedSet, each point is sorted on the x axis.  
The server listens on http://localhost:5000  
The Swagger interface can be accessed at http://localhost:5000/swagger

## Run Locally  

Download the latest release at https://github.com/thiccnugget/CartesianPlaneAPI/releases/ and execute the CartesianPlaneAPI.exe file.  
Note that the API has been compiled for windows only.

You can use any program to make HTTP requests, Postman or any similar software can also be used.  
Also, you can interact with the API directly on the server using the Swagger interface at http://localhost:5000/swagger  


Each endpoint has the following base URL: http://localhost:5000/  

Example: [POST] http://localhost:5000/point

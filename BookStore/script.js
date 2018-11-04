var scotchApp = angular.module('scotchApp', ['ngRoute']);

// configure our routes
scotchApp.config(function ($routeProvider) {
    $routeProvider

        // route for the home page
        .when('/', {
            templateUrl: '/pages/Author.html',
            controller: 'mainController'
        })

        // route for the Book page
        .when('/Book', {
            templateUrl: '/pages/Book.html',
            controller: 'aboutController'
        })

        // route for the Author page
        .when('/Author', {
            templateUrl: '/pages/Author.html',
            controller: 'contactController'
        });
});

// create the controller and inject Angular's $scope
scotchApp.controller('maincontroller', function ($scope,AuthorService) {
    // create a message to display in our view
    $scope.message = 'Everyone come and see how good I look!';
    
    AuthorService.GetAuthor().then(function (d) {
        debugger;
        console.log(d.data.list);
        $scope.CustomerList =d.data.list;
    });

   
    //$http({
    //    url: "/api/Author",
    //    dataType: 'json',
    //    method: 'GET',
    //    data: '',
    //    headers: {
    //        "Content-Type": "application/json"
    //    }
    //}).success(function (response) {
    //    debugger;
    //    $scope.CustomerList = response;
    //}).error(function (error) {
    //         alert(error);
    //     });
});



scotchApp.factory("AuthorService", ["$http", function ($http) {
    var fac = {};

    fac.GetAuthor = function () {
        return $http.get("/api/Author");
    }

    //fac.GetPlayerById = function (id) {
    //    return $http.get("/Player/GetPlayerById", { params: { id: id } });
    //}

    //fac.AddPlayer = function (player) {
    //    //alert(player.Name);
    //    $http.post("/Player/AddPlayer", player).then(function (response) {
    //        alert(response.data.status);
    //    });
    //}
    //fac.UpdatePlayer = function (player) {
    //    $http.post("Player/UpdatePlayer", player).then(function (response) {
    //        alert(response.data.result);
    //    });
    //}
    //fac.DeletePlayer = function (id) {
    //    $http.post("Player/DeletePlayer", { id: id }).then(function (response) {
    //        alert(response.data.result);
    //    });
    //}

    return fac;
}])
scotchApp.controller('AuthorController', function ($scope) {
    $scope.msg = 'Look! I am an about page.';
});

scotchApp.controller('BookController', function ($scope) {
    $scope.message = 'Contact us! JK. This is just a demo.';
});
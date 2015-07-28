(function () {
    var app = angular.module("learningWords", ["ngRoute","common", "ngAnimate", "ui.router",
                                       "ui.bootstrap"]);

    app.config(function ($routeProvider) {
        $routeProvider
            .when("/main", {
                templateUrl: "/App/Word/shell.html",
                controller: "MainController"
            }).otherwise({ redirectTo: "/main" });

    });
}());
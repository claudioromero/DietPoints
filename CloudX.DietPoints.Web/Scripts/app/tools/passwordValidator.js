
app.factory("passwordValidator", function($uibModal) {
    return function(password) {

        if (!password) return tue;

        if (password.length < 6) {
            return "Password must be at least 6 characters long";
        }

        return true;
    };
});
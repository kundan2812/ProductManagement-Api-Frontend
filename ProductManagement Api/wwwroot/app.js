var app = angular.module("productApp", []);

app.controller("productCtrl", function ($scope, $http) {

    $scope.items = [];
    $scope.form = {};  // FIXED (form instead of item)
    $scope.isEdit = false;

    // SHOW MESSAGE
    $scope.showMessage = function (msg, type) {
        $scope.message = msg;
        $scope.success = type === "success";

        setTimeout(() => {
            $scope.message = null;
            $scope.$apply();
        }, 2000);
    };

    // LOAD ITEMS
    $scope.loadItems = function () {
        $http.get("https://localhost:7128/api/Items/ListItems").then(
            function (response) {
                $scope.items = response.data;
            },
            function () {
                $scope.showMessage("Failed to load items", "error");
            }
        );
    };

    // FILE → BASE64
    $scope.onFileChange = function (input) {
        let file = input.files[0];
        if (!file) return;

        let reader = new FileReader();
        reader.onload = function (e) {
            let base64 = e.target.result.split(',')[1];
            $scope.form.photoBase64 = base64;
            $scope.$apply();
        };
        reader.readAsDataURL(file);
    };

    // SAVE ITEM
    $scope.saveItem = function () {

        if (!$scope.form.itemCode || !$scope.form.description) {
            $scope.showMessage("Please fill required fields", "error");
            return;
        }

        if ($scope.isEdit) {
            // UPDATE
            $http.put("https://localhost:7128/api/Items/EditItem", $scope.form).then(
                function () {
                    $scope.showMessage("Item updated successfully!", "success");
                    $scope.cancelForm();
                    $scope.loadItems();
                }
            );
        } else {
            // ADD
            $http.post("https://localhost:7128/api/Items/AddItem", $scope.form).then(
                function () {
                    $scope.showMessage("Item added successfully!", "success");
                    $scope.cancelForm();
                    $scope.loadItems();
                }
            );
        }
    };

    // EDIT
    $scope.editItem = function (item) {
        $scope.form = angular.copy(item);
        $scope.isEdit = true;
        $scope.showForm = true;
    };

    // DELETE
    $scope.deleteItem = function (code) {
        if (!confirm("Are you sure?")) return;

        $http.delete("https://localhost:7128/api/Items/DeleteItem/" + code).then(
            function () {
                $scope.showMessage("Item deleted successfully!", "success");
                $scope.loadItems();
            }
        );
    };

    // SHOW FORM
    $scope.showAddForm = function () {
        $scope.form = {};
        $scope.isEdit = false;
        $scope.showForm = true;
    };

    // CANCEL
    $scope.cancelForm = function () {
        $scope.showForm = false;
        $scope.form = {};
        $scope.isEdit = false;
    };

    // AUTO LOAD ITEMS
    $scope.loadItems();
});

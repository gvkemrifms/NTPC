<%@ Page Language="C#" AutoEventWireup="true" CodeFile="zModalPopupTest.aspx.cs" Inherits="zModalPopupTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <script src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript" src="http://maps.google.com/maps/api/js?places=Rajasthan&libraries=places&key=d1CkkvZsTH1jgTgvsZvvYRaSDSc="></script>
    <script src="js/locationpicker.js"></script>

    <title>:: GVK EMRI ::</title>

    <style>
        .pac-container:after { content: none !important; }

        .hightmap { height: 400px !important; }

        .modal {
            background-color: rgb(0, 0, 0); /* Fallback color */
            background-color: rgba(0, 0, 0, 0.4); /* Black w/ opacity */
            display: none; /* Hidden by default */
            height: 100%; /* Full height */
            left: 0;
            overflow: auto; /* Enable scroll if needed */
            padding-top: 100px; /* Location of the box */
            position: fixed; /* Stay in place */
            top: 0;
            width: 100%; /* Full width */
            z-index: 1; /* Sit on top */
        }

        /* Modal Content */

        .modal-content {
            background-color: #fefefe;
            border: 1px solid #888;
            margin: auto;
            padding: 20px;
            width: 80%;
        }

        /* The Close Button */

        .close {
            color: #aaaaaa;
            float: right;
            font-size: 28px;
            font-weight: bold;
        }
    </style>
</head>
<body>
<form id="form1" runat="server">

    <button id="myBtn">click</button>


    <div id="myModal" class="modal">
        <div class="modal-content">
            <span class="close">×</span>

            <input type="text" name="address" id="address" style="width: 40%">
            <input type="text" id="us2lat" value=""/><input type="text" id="us2lon" value=""/>
            <div id="us2" style="width: 100%; height: 100%;" class=""></div>
        </div>
    </div>


    <script>
        function abc() {
            $('#us2').locationpicker({
                location: { latitude: '22.6754807', longitude: '88.0883874' },
                radius: 20,
                zoom: 7,
                inputBinding: {
                    latitudeInput: $('#us2lat'),
                    longitudeInput: $('#us2lon'),
                    radiusInput: $('#us2radius'),
                    locationNameInput: $('#address')
                },
                enableAutocomplete: true,
                onchanged: function(currentLocation, radius, isMarkerDropped) {
                    $('#us2lon').val(currentLocation.longitude);

                }
            });
        }


        var modal = document.getElementById('myModal');
        var btn = document.getElementById("myBtn");
        var span = document.getElementsByClassName("close")[0];
        btn.onclick = function() {
            abc();
            modal.style.display = "block";
        };
        span.onclick = function() {
            modal.style.display = "none";
        };
        window.onclick = function(event) {
            if (event.target === modal) {
                modal.style.display = "none";
            }
        };
    </script>

</form>
</body>
</html>

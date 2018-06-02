//// It is used to validate the Id text box.
//function valid(f)
//{
//    !(/^["0123456789"]*$/i).test(f.value)?f.value = f.value.replace(/[^"0123456789"]/ig,''):null;
//    return false;
//} 
//function valid11(f)
//{
//    !(/^["abcdefghijklmnopqrstuvwxyz0123456789"]*$/i).test(f.value)?f.value = f.value.replace(/[^"abcdefghijklmnopqrstuvwxyz0123456789"]/ig,''):null;
//    return false;
//} 

//// It is used to validate the Name text box.
//function valid_name(f) 
//{
//    !(/^["abcdefghijklmnopqrstuvwxyz"]*$/i).test(f.value)?f.value = f.value.replace(/[^"abcdefghijklmnopqrstuvwxyz"]/ig,''):null;
//    return false;
//} 

//------------------------------------------------Created By Ismail---------------------------------------------//

            function OnlyAlphaNumeric(e) 
              {
                var keycode; 
                if (window.event) keycode = window.event.keyCode;
                else if (event) keycode = event.keyCode; 
                else if (e) keycode = e.which;
                else return true;if( (keycode >= 47 && keycode <= 57) || (keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122) ) 
                {
                return true; 
                }
                else
                {
                return false; 
                }
                return true; 
              }
            function OnlyAlphabets(myfield, e, dec) 
              {
	            var key;
	            var keychar;
	            if (window.event)
		            key = window.event.keyCode;
	            else if ( e )
		            key = e.which;
	            else
		            return true;
	            keychar = String.fromCharCode(key);
	            if (((" !@#$%^&*()_+=-';{}[]|?<>:,/\".1234567890").indexOf(keychar) > -1))
		        return false;
	            else
		        return true;
              }
            function OnlyNumbers(evt)
              {
                var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;
                else
                    return true;
              }
              //This function is for Agency Type Validation in Agency Details page
              
              function validationAdd()
                 {
                 
                 if(document.getElementById("<%=txtAgencyType.ClientID %>").value==0)
                 {
                    alert("Please Enter Agency Type");
                    document.getElementById("<%=txtAgencyType.ClientID %>").focus();
                    return false;
                 }
                 if(document.getElementById("<%=txtSubType.ClientID %>").value==0)
                 {
                    alert("Please Enter Sub Type ");
                    document.getElementById("<%=txtSubType.ClientID %>").focus();
                    return false;
                 }
                  if(document.getElementById("<%=txtManufacturer.ClientID %>").value==0)
                 {
                    alert("Please Enter Anufacturer");
                    document.getElementById("<%=txtManufacturer.ClientID %>").focus();
                    return false;
                 }
           }
           
           //Javascript functions by Bhushan Madan
           
           function alphanumeric_only(e) 
             {
                var keycode; 
                if (window.event) keycode = window.event.keyCode;
                else if (event) keycode = event.keyCode; 
                else if (e) keycode = e.which;
                else return true;if( (keycode >= 48 && keycode <= 57) || (keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122) ) 
                {
                    return true; 
                }
                else
                {
                    return false; 
                }
                 return true; 
            }

function alphanumeric_only_withspace(e) 
             {
                var keycode; 
                if (window.event) keycode = window.event.keyCode;
                else if (event) keycode = event.keyCode; 
                else if (e) keycode = e.which;
                else return true;if( (keycode >= 48 && keycode <= 57) || (keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122) || (keycode == 32))
                {
                    return true; 
                }
                else
                {
                    return false; 
                }
                 return true; 
            }


            function alpha_only(e) 
            {
                var keycode; 
                if (window.event) keycode = window.event.keyCode;
                else if (event) keycode = event.keyCode; 
                else if (e) keycode = e.which;
                else return true;if((keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122)) 
                {
                    return true; 
                }
                else
                {
                    return false; 
                }
                return true; 
            }
         
            function alpha_only_withspace(e) 
            {
            var keycode; 
            if (window.event) keycode = window.event.keyCode;
            else if (event) keycode = event.keyCode; 
            else if (e) keycode = e.which;
            else return true;if((keycode >= 65 && keycode <= 90) || (keycode >= 97 && keycode <= 122)|| (keycode == 32)) 
            {
                return true; 
            }
            else
            {
                return false; 
            }
            return true; 
            }


function remark(e) 
             {
                var keycode; 
                if (window.event) keycode = window.event.keyCode;
                else if (event) keycode = event.keyCode; 
                else if (e) keycode = e.which;
                else return true;if((keycode != 34) && (keycode != 39) ) 
                {
                    return true; 
                }
                else
                {
                    return false; 
                }
                 return true; 
            }




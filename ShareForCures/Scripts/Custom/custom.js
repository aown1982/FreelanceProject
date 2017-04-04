
$(document).ready(function () {
    $('#createAccountForm').bootstrapValidator({
        // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            firstName: {
                validators: {
                    stringLength: {
                        min: 2,
                    },
                    notEmpty: {
                        message: 'Please enter your first name'
                    }
                }
            },
            lastName: {
                validators: {
                    stringLength: {
                        min: 2,
                    },
                    notEmpty: {
                        message: 'Please enter your last name'
                    }
                }
            },
            signupEmail: {
                validators: {
                    notEmpty: {
                        message: 'Please enter your email address'
                    },
                    emailAddress: {
                        message: 'Please enter a valid email address'
                    }
                }
            },
            signupConfirmEmail: {
                validators: {
                    notEmpty: {
                        message: 'Please enter your email address'
                    },
                    identical: {
                        field: "signupEmail",
                        message: "Email and Confirmation Email must match"
                    }
                }
            },
            invitationCode: {
                validators: {
                    notEmpty: {
                        message: 'Please enter your Invitation Code'
                    },
                    identical: {
                        field: "invitationCode",
                        message: "Please enter your Invitation Code"
                    }
                }
            },
            password: {
                message: 'The password is not valid',
                threshold: 8,
                validators: {
                    notEmpty: {
                        message: 'A password is required'
                    },
                    regexp: {
                        regexp: /^(?=.*[A-Z])(?=.*[!@#$&_/\*])(?=.*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,128}/,
                        message: 'The password must be 8 characters in length and contain at least 1 Capital & 3 lower case letters, 1 Special Character, and 1 number.'
                    }
                }
            },
            confirmPassword: {
                message: 'The password is not valid',
                validators: {
                    notEmpty: {
                        message: 'A password is required'
                    },
                    identical: {
                        field: "password",
                        message: "Password and Confirmation Password must match"
                    }
                }
            },
            'tcConsent': {
                container: '#tcpErrMsg',
                validators: {
                    choice: {
                        message: 'You must agree to the Terms & Conditions & the Privacy Policy to create an account.',
                        min: 1,
                        max: 1
                    }
                }
            }
        }


    }).on('submit', function (e) {
        if (e.isDefaultPrevented()) {
            e.preventDefault();
        } else {
            var model = {
                FirstName: jQuery("#firstName").val(),
                LastName: jQuery("#lastName").val(),
                Email: jQuery("#signupEmail").val(),
                PasswordHash: jQuery("#confirmPassword").val(),
                Username: jQuery("#signupEmail").val(),
                InvitationCode: jQuery("#invitationCode").val(),
                SocialType: jQuery('input:hidden[name=SocialType]').val(),
                SocialUserId: jQuery('input:hidden[name=SocialUserId]').val(),
                AccessToken: $('input:hidden[name=AccessToken]').val(),
                PublicToken: $('input:hidden[name=PublicToken]').val(),
                SourceUserIDToken: $('input:hidden[name=IdToken]').val(),
                RefreshToken: $('input:hidden[name=RefreshToken]').val(),
                ExpiresIn: $('input:hidden[name=ExpiresIn]').val()

                //SocialType: 'USER ENTERED'
            };
            if (model.SocialType === "") {
                model.SocialType = 'USER ENTERED';
                model.AccessToken = null;
                model.PublicToken = null;
                model.SourceUserIDToken = null;
                model.RefreshToken = null;
                model.ExpiresIn = null;
            }
            $.blockUI({
                baseZ: 30009
            });
            $.ajax({
                type: "POST",
                data: JSON.stringify(model),
                url: "/User/RegisterUser",
                contentType: "application/json"
            }).done(function (res) {
                if (res === "Invitation Code you entered is invalid.") {
                    $("#error").remove();
                    $("#alert").append('<span id="error"> The invitation code you entered is invalid.</span>');
                    $('#alert').fadeIn('slow');
                    $.unblockUI();
                    e.preventDefault();
                    document.location.href = "#alert";
                    return;
                } else if (res === "Email address you entered is not available. Please try another.") {
                    $("#error").remove();
                    $("#alert").append('<span id="error"> The email address you entered is already in use. Please use forgot password if you need help logging in.</span>');
                    $('#alert').fadeIn('slow');
                    e.preventDefault();
                    $.unblockUI();
                    document.location.href = "#alert";
                    return;
                } else if (res === "Invitation Code you entered is already used.") {
                    $("#error").remove();
                    $("#alert").append('<span id="error"> The invitation code you entered has already used.</span>');
                    $('#alert').fadeIn('slow');
                    e.preventDefault();
                    $.unblockUI();
                    document.location.href = "#alert";
                    return;
                } else if (res === "This user is already registered. Please try another.") {
                    $("#error").remove();
                    $("#alert").append('<span id="error"> This user is already registered. </span>');
                    $('#alert').fadeIn('slow');
                    e.preventDefault();
                    $.unblockUI();
                    document.location.href = "#alert";
                    return;
                } else if (res === "SUCCESS") {
                    $("#loginForm").hide();
                    $('#createAccount').hide();
                    $("#forgotPassword").hide();
                    $('#resetPassword').hide();
                    $("#serverMsgContainer").hide();
                    $('#welcomeNewUserContainer').removeClass('hidden');
                    $("#welcomeNewUserContainer").show();
                    $.unblockUI();
                    var $form = $(e.target);
                }
                else {
                    $("#error").remove();
                    $("#alert").append('<span id="error"> Unfortunately, something went wrong. We have logged the error. Please try again later.</span>');
                    $('#alert').fadeIn('slow');
                    e.preventDefault();
                    $.unblockUI();
                    document.location.href = "#alert";
                    return;
                }
            });
            e.preventDefault();
        }
    });

    $('#frmLogin').bootstrapValidator({
        // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            email: {
                validators: {
                    notEmpty: {
                        message: 'Please enter your email address'
                    },
                    emailAddress: {
                        message: 'Please enter a valid email address'
                    }
                }
            },
            password: {
                message: 'The password is not valid',
                validators: {
                    notEmpty: {
                        message: 'The password is required'
                    },
                    //stringLength: {
                    //    min: 6,
                    //    max: 30,
                    //    message: 'The password must be more than 6 and less than 30 characters long'
                    //},
                    regexp: {
                        //regexp: /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,10}/,
                        regexp: /^(?=.*[A-Z])(?=.*[!@#$&_/\*])(?=.*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,128}/,
                        message: 'The password must be 8 characters in length and contain at least 1 Capital & 3 lower case letters, 1 Special Character, and 1 number.'
                    }
                }
            }
        }
    })
        .on('submit', function (e) {
            if (e.isDefaultPrevented()) {
                e.preventDefault();
            } else {
                //var $form = $(e.target);
                var email = $("#email").val();
                var password = $("#password").val();
                var remember = false;

                if ($("#cbRememberMe").is(':checked'))
                    remember = true;
                else
                    remember = false;

                $.blockUI({
                    baseZ: 30009
                });
                var model = {
                    Username: email,
                    Password: password,
                    IpAddress: "",
                    RememberMe: remember
                };
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(model),
                    url: "/User/Login",
                    contentType: "application/json"
                }).done(function (data) {
                    if (data === "Invalid Email or Password. Please try again.") {
                        $("#error").remove();
                        $("#alertLogin").append('<span id="error"> Invalid Email or Password. Please try again.</span>');
                        $('#alertLogin').fadeIn('slow');
                        $.unblockUI();
                        document.location.href = "#alertLogin";
                        e.preventDefault();
                        return;
                    } else if (data === "Unauthorize user. Please contact your administrator.") {
                        $("#error").remove();
                        $("#alertLogin").append('<span id="error"> Unauthorize user. Please contact your administrator.</span>');
                        $('#alertLogin').fadeIn('slow');
                        $.unblockUI();
                        document.location.href = "#alertLogin";
                        e.preventDefault();
                        return;
                    } else if (data === "Unfortunately, something went wrong. We have logged this error. Please try again later.") {
                        $("#error").remove();
                        $("#alertLogin").append('<span id="error">Unfortunately, something went wrong. We have logged this error. Please try again later.</span>');
                        $('#alertLogin').fadeIn('slow');
                        $.unblockUI();
                        document.location.href = "#alertLogin";
                        e.preventDefault();
                        return;
                    }
                    window.location = data;

                });
                e.preventDefault();
            }
        });


    $('#forgotPasswordForm').bootstrapValidator({
        // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {

            forgotEmail: {
                validators: {
                    notEmpty: {
                        message: 'Please enter your email address'
                    },
                    emailAddress: {
                        message: 'Please enter a valid email address'
                    }
                }
            }
        }
    })
           .on('submit', function (e) {
               if (e.isDefaultPrevented()) {
                   e.preventDefault();
               } else {
                   e.stopPropagation();
                   e.preventDefault();
                   var modal = {
                       email: jQuery("#forgotEmail").val(),
                   };
                   $.blockUI({
                       baseZ: 30009
                   });

                   $.ajax({
                       type: "POST",
                       data: JSON.stringify(modal),
                       url: "/User/ForgotPassword",
                       contentType: "application/json"
                   }).done(function (res) {
                       if (res === "Email you entered is invalid.") {
                           $("#error").remove();
                           $("#alertPassword").append('<span id="error"> Please make sure email address you entered is correct.</span>');
                           $('#alertPassword').fadeIn('slow');
                           $.unblockUI();
                           document.location.href = "#alert";
                           return;

                       } else if (res === "FAIL") {
                           $("#error").remove();
                           $("#alertPassword").append('<span id="error"> Something went wrong, please contact administrator!</span>');
                           $('#alertPassword').fadeIn('slow');
                           $.unblockUI();
                           document.location.href = "#alert";
                           return;
                       }
                       else if (res === "SUCCESS") {
                           $("#error").remove();
                           $.unblockUI();
                           $('#forgotPassword').hide();
                           $("#serverMsgContainer").show();
                           $('#msgInfo').html('Thank you for submitting your password reset request. An email with password reset instructions will be sent if we have a user account associated with the email address you entered.');

                           var $form = $(e.target);
                           return;
                       }
                   });

                   return false;
               }
           });

    $('#resetPasswordForm').bootstrapValidator({
        // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            txtCurrentPassword: {
                message: 'The password is not valid',
                validators: {
                    notEmpty: {
                        message: 'Current password is required'
                    },
                    regexp: {
                        regexp: /^(?=.*[A-Z])(?=.*[!@#$&_/\*])(?=.*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,128}/,
                        message: 'The password must be 8 characters in length and contain at least 1 Capital & 3 lower case letters, 1 Special Character, and 1 number.'
                    }
                }
            },
            txtResetPassword: {
                message: 'The password is not valid',
                validators: {
                    notEmpty: {
                        message: 'The password is required'
                    },
                    regexp: {
                        regexp: /^(?=.*[A-Z])(?=.*[!@#$&_/\*])(?=.*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,128}/,
                        message: 'The password must be 8 characters in length and contain at least 1 Capital & 3 lower case letters, 1 Special Character, and 1 number.'
                    },
                    different: {
                        field: 'txtCurrentPassword',
                        message: 'New password must be different from current password'
                    }
                }
            }, txtResetPasswordAgain: {
                message: 'The password is not valid',
                validators: {
                    notEmpty: {
                        message: 'The password is required'
                    },
                    identical: {
                        field: "txtResetPassword",
                        message: "Password and Confirmation Password must match"
                    }
                }
            }
        }
    })
        .on('submit', function (e) {
            if (e.isDefaultPrevented()) {
                e.preventDefault();
            } else {
                e.preventDefault();
                var modal = {
                    CurrentPassword: jQuery("#txtCurrentPassword").val(),
                    NewPassword: jQuery("#txtResetPassword").val(),
                    ConfirmPassword: jQuery("#txtResetPasswordAgain").val()
                };
                $.blockUI({
                    baseZ: 30009
                });
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(modal),
                    url: "/User/RestPassword",
                    contentType: "application/json"
                }).done(function (res) {
                    if (res === "Please make sure all information you entered is correct.") {
                        $("#error").remove();
                        $("#alertPassword").append('<span id="error"> Please make sure all information you entered is correct.</span>');
                        $('#alertPassword').fadeIn('slow');
                        $.unblockUI();
                        document.location.href = "#alert";
                        return;

                    } else if (res === "Please login to proceed.") {
                        $("#error").remove();
                        $("#alertPassword").append('<span id="error"> Please login to proceed.</span>');
                        $('#alertPassword').fadeIn('slow');
                        $.unblockUI();
                        document.location.href = "#alert";
                        return;

                    } else if (res === "Passwords do not match. Please try again.") {
                        $("#error").remove();
                        $("#alertPassword").append('<span id="error"> Passwords do not match. Please try again.</span>');
                        $('#alertPassword').fadeIn('slow');
                        $.unblockUI();
                        document.location.href = "#alert";
                        return;

                    } else if (res === "Current password you entered is wrong. Please try again.") {
                        $("#error").remove();
                        $("#alertPassword").append('<span id="error"> Current password you entered is wrong. Please try again.</span>');
                        $('#alertPassword').fadeIn('slow');
                        $.unblockUI();
                        document.location.href = "#alert";
                        return;

                    } else if (res === "Something went wrong. Please try again.") {
                        $("#error").remove();
                        $("#alertPassword").append('<span id="error"> Something went wrong. Please try again.</span>');
                        $('#alertPassword').fadeIn('slow');
                        $.unblockUI();
                        document.location.href = "#alert";
                        return;

                    } else if (res === "FAILED") {
                        $("#error").remove();
                        $("#alertPassword").append('<span id="error"> Something went wrong. Please try again later.</span>');
                        $('#alertPassword').fadeIn('slow');
                        $.unblockUI();
                        document.location.href = "#alert";
                        return;

                    } else if (res === "SUCCESS") {
                        $("#error").remove();
                        $.unblockUI();
                        $('#resetPassword').hide();
                        $("#serverMsgContainer").show();
                        $('#msgInfo').html('Thank you for resetting your password!');
                        var $form = $(e.target);
                    }
                });

            }
        });

    $('#basicInfoForm').bootstrapValidator({
        // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            txtFirstName: {
                validators: {
                    stringLength: {
                        min: 2,
                    },
                    notEmpty: {
                        message: 'Please enter your first name'
                    }
                }
            },
            txtLastName: {
                validators: {
                    stringLength: {
                        min: 2,
                    },
                    notEmpty: {
                        message: 'Please enter your last name'
                    }
                }
            },
            txtNewEmail: {
                validators: {
                    notEmpty: {
                        message: 'Please enter your email address'
                    },
                    emailAddress: {
                        message: 'Please enter a valid email address'
                    }
                }
            }
        }
    })
        .on('submit', function (e) {
            if (e.isDefaultPrevented()) {
                e.preventDefault();
            } else {
                var modal = {
                    FirstName: jQuery("#txtFirstName").val(),
                    LastName: jQuery("#txtLastName").val(),
                    Email: jQuery("#txtNewEmail").val()
                };
                $.blockUI({
                    baseZ: 30009
                });
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(modal),
                    url: "/User/UpdateUserInfo",
                    contentType: "application/json"
                }).done(function (res) {
                    if (res === "Please login to proceed.") {
                        $("#error").remove();
                        $("#alertAccountInfo").append('<span id="error"> Please login to proceed.</span>');
                        $('#alertAccountInfo').fadeIn('slow');
                        $.unblockUI();
                        document.location.href = "#alertAccountInfo";
                        return;

                    } else if (res === "Email address you entered is not available. Please try another.") {
                        $("#error").remove();
                        $("#alertAccountInfo").append('<span id="error"> Email address you entered is not available. Please try another.</span>');
                        $('#alertAccountInfo').fadeIn('slow');
                        $.unblockUI();
                        document.location.href = "#alertAccountInfo";
                        return;

                    } else if (res === "FAILED") {
                        $("#error").remove();
                        $("#alertAccountInfo").append('<span id="error"> Something went wrong. Please try again later.</span>');
                        $('#alertAccountInfo').fadeIn('slow');
                        $.unblockUI();
                        document.location.href = "#alertAccountInfo";
                        return;

                    } else if (res === "SUCCESS") {
                        $("#error").remove();
                        $.unblockUI();
                        $('#resetPassword').hide();
                        $('#updateBasicInfo').hide();
                        $('#updateDemographics').hide();
                        $('#updateLocationLanguageInfo').hide();
                        $('#deleteAccount').hide();
                        $('#serverMsgContainer').removeClass('hidden');;
                        $('#serverMsgContainer').show();

                        $('#msgInfo').html('Your name and email address are updated successully.');
                        e.preventDefault();
                        return;
                        //var $form = $(e.target);
                    }
                });
                e.preventDefault();
            }
        });

    $('#deleteAccountForm').bootstrapValidator({
        // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {

        }
    })
        .on('submit', function (e) {
            if (e.isDefaultPrevented()) {
                e.preventDefault();
            } else {
                //$('#deleteAccount').hide();
                //$('#serverMsgContainer').show();
                //$('#msgInfo').html('Your account deleted successfully.');
                //setTimeout(function () {
                //    window.location = "/home/Logout";
                //}, 1000);

                //e.preventDefault();
                //var $form = $(e.target);
                $.blockUI({
                    baseZ: 30009
                });
                $.ajax({
                    type: "GET",
                    url: "/User/DeactivateAccount"
                }).done(function (res) {
                    if (res === "FAILED") {
                        $("#error").remove();
                        $("#alertAccountInfo").append('<span id="error"> Something went wrong. Please try again later.</span>');
                        $('#alertAccountInfo').fadeIn('slow');
                        $.unblockUI();
                        document.location.href = "#alertAccountInfo";
                        return;

                    } else if (res === "SUCCESS") {
                        $("#error").remove();
                        $.unblockUI();
                        $('#deleteAccount').hide();
                        $('#serverMsgContainer').show();
                        $('#msgInfo').html('Your account has been deactivated.');
                        setTimeout(function () {
                            window.location = "/home/Logout";
                        }, 1000);

                        e.preventDefault();
                        var $form = $(e.target);
                        return;
                    }
                });
            }
        });

    $('#locationLanguageForm').bootstrapValidator({
        // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {

        }
    })
        .on('submit', function (e) {
            if (e.isDefaultPrevented()) {
                e.preventDefault();

            } else {

                var ddl_Language = $("#ddl_Language").data("kendoDropDownList");
                var ddl_State = $("#ddl_State").data("kendoDropDownList");


                   var modal = {
                    LocationId: ddl_State.value(),
                    LanguageId: ddl_Language.value()
                };
                $.blockUI({
                    baseZ: 30009
                });
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(modal),
                    url: "/User/UpdateUserLanguageAndLocation",
                    contentType: "application/json"
                }).done(function (res) {
                    if (res === "FAILED") {
                        $("#error").remove();
                        $("#alertPassword").append('<span id="error"> Something went wrong. Please try again.</span>');
                        $('#alertPassword').fadeIn('slow');
                        $.unblockUI();
                        document.location.href = "#alert";
                        return;

                        return;

                    } else if (res === "SUCCESS") {
                        $("#error").remove();
                        $.unblockUI();
                        $('#resetPassword').hide();
                        $('#updateBasicInfo').hide();
                        $('#updateDemographics').hide();
                        $('#updateLocationLanguageInfo').hide();
                        $('#deleteAccount').hide();
                        $('#serverMsgContainer').removeClass('hidden');;
                        $('#serverMsgContainer').show();

                        $('#msgInfo').html('Your Location & Language have been updated successfully.');

                        $("#location").text(ddl_State.text());
                        $("#language").text(ddl_Language.text());
                        $("#locationId").val(ddl_State.value());
                        $("#languageId").val(ddl_Language.value());

                        e.preventDefault();
                        return;
                        //var $form = $(e.target);
                    }
                });
                e.preventDefault();
            }
        });

   $('#demographicsForm').bootstrapValidator({
        // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {

        }
    })
        .on('submit', function (e) {
            if (e.isDefaultPrevented()) {
                e.preventDefault();

            } else {

                var dp_dob = $("#dp_dob").data("kendoDatePicker"); 
                var ddl_Gender = $("#ddl_Gender").data("kendoDropDownList");


                   var modal = {
                       DOB: dp_dob.value(),
                       GenderId: ddl_Gender.value()
                };
                $.blockUI({
                    baseZ: 30009
                });
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(modal),
                    url: "/User/UpdateUserDobAndGender",
                    contentType: "application/json"
                }).done(function (res) {
                    if (res === "FAILED") {
                        $("#error").remove();
                        $("#alertPassword").append('<span id="error"> Something went wrong. Please try again.</span>');
                        $('#alertPassword').fadeIn('slow');
                        $.unblockUI();
                        document.location.href = "#alert";
                        return;

                        return;

                    } else if (res === "SUCCESS") {
                        $("#error").remove();
                        $.unblockUI();
                        $('#resetPassword').hide();
                        $('#updateBasicInfo').hide();
                        $('#updateDemographics').hide();
                        $('#updateLocationLanguageInfo').hide();
                        $('#deleteAccount').hide();
                        $('#serverMsgContainer').removeClass('hidden');;
                        $('#serverMsgContainer').show();

                        $('#msgInfo').html('Your Date of Birth & Gender have been updated successfully.');

                        e.preventDefault();
                        return;
                        //var $form = $(e.target);
                    }
                });
                e.preventDefault();
            }
        });

    $('#changeEmailPasswordForm').bootstrapValidator({
        // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {

            txtResetPassword: {
                message: 'The password is not valid',
                validators: {
                    notEmpty: {
                        message: 'The password is required'
                    },
                    regexp: {
                        regexp: /^(?=.*[A-Z])(?=.*[!@#$&_/\*])(?=.*[0-9])(?=.*[a-z].*[a-z].*[a-z]).{8,128}/,
                        message: 'The password can only consist of 1 Capital & three lower case letters, 1 Special Character , 1 number'
                    }
                }
            }, txtResetPasswordAgain: {
                message: 'The password is not valid',
                validators: {
                    notEmpty: {
                        message: 'The password is required'
                    },
                    identical: {
                        field: "txtResetPassword",
                        message: "Password and Confirmation Password must match"
                    }
                }
            }
        }
    })
       .on('submit', function (e) {
           if (e.isDefaultPrevented()) {
               e.preventDefault();
           } else {
               e.preventDefault();
               $.blockUI({
                   baseZ: 30009
               });
               var modal = {
                   NewPassword: jQuery("#txtResetPassword").val(),
                   ConfirmPassword: jQuery("#txtResetPasswordAgain").val(),
                   ResetCodeID: getParameterByName("resetCode"),
                   ExternalUserID: getParameterByName("externalId"),
                   UserID: jQuery("#User").val()
               };

               $.ajax({
                   type: "POST",
                   data: JSON.stringify(modal),
                   url: "/User/ForgotPasswordReset",
                   contentType: "application/json"
               }).done(function (res) {
                   if (res === "Code has already been used!") {
                       $("#error").remove();
                       $("#alertAccountInfo").append('<span id="error">Code has already been used!</span>');
                       $('#alertAccountInfo').fadeIn('slow');
                       $.unblockUI();
                       document.location.href = "#alertAccountInfo";
                       return;
                   }
                   else if (res === "Reset Code has expired!") {
                       $("#error").remove();
                       $("#alertAccountInfo").append('<span id="error"> Reset Code has expired, please click on the forgot password again to get the latest link!</span>');
                       $('#alertAccountInfo').fadeIn('slow');
                       $.unblockUI();
                       document.location.href = "#alertAccountInfo";
                       return;
                   }
                   else if (res === "Invalid Reset Code entered!") {
                       $("#error").remove();
                       $("#alertAccountInfo").append('<span id="error"> Invalid Reset Code provided, please contact support team!</span>');
                       $('#alertAccountInfo').fadeIn('slow');
                       $.unblockUI();
                       document.location.href = "#alertAccountInfo";
                       return;
                   }
                   else if (res === "Invalid password details entered, please contact administrator!") {
                       $("#error").remove();
                       $("#alertAccountInfo").append('<span id="error"> Invalid password details entered, please contact support team!</span>');
                       $('#alertAccountInfo').fadeIn('slow');
                       $.unblockUI();
                       document.location.href = "#alertAccountInfo";
                       return;
                   }
                   else if (res === "SUCCESS") {
                       $("#error").remove();
                       $('#alertAccountInfo').hide();
                       $.unblockUI();
                       $("#changeEmailPassword").hide();
                       $("#divMsg").show();
                       $("#msgResetInfo").html('Thank you for resetting your password.');
                       var $form = $(e.target);
                       return;
                   }
                   else {
                       $("#error").remove();
                       $("#alertAccountInfo").append('<span id="error"> Something went wrong, please try again!</span>');
                       $('#alertAccountInfo').fadeIn('slow');
                       $.unblockUI();
                       document.location.href = "#alertAccountInfo";
                       return;
                   }
               });
           }
       });


    $('#emailForm').bootstrapValidator({
        // To use feedback icons, ensure that you use Bootstrap v3.1.0 or later
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            txt_names: {
                validators: {
                    notEmpty: {
                        message: 'The name is required'
                    }
                }
            },
            txt_email: {
                validators: {
                    notEmpty: {
                        message: 'The email is required'
                    },
                    regexp: {
                        regexp: /^[a-zA-Z0-9.!#$%&’*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/,
                        message: 'Please enter valid email'
                    }
                }
            },
            txt_message: {
                validators: {
                    notEmpty: {
                        message: 'The message is required'
                    }
                }
            }
        }
    })
           .on('submit', function (e) {
               if (e.isDefaultPrevented()) {
                   e.preventDefault();
               }
               else {

                   $.blockUI({
                       baseZ: 30009
                   });
                   var modal = {
                       name: $("#txt_names").val(),
                       email: $("#txt_email").val(),
                       message: $("#txt_message").val(),
                       emailTo: $("#EmailTo").val()
                   };

                   $.ajax({
                       type: "POST",
                       data: JSON.stringify(modal),
                       url: "/User/SendEmail",
                       contentType: "application/json"
                   }).done(function (res) {
                       if (res === "FAILED") {
                           $("#error").remove();
                           $("#alertAccountInfo").append('<span id="error"> Something went wrong. Please try again.</span>');
                           $('#alertAccountInfo').fadeIn('slow');
                           $('#divMsg').hide();
                           $.unblockUI();
                           document.location.href = "#alertAccountInfo";
                           return;

                       } else if (res === "SUCCESS") {
                           $("#alertAccountInfo").hide();
                           $('#msgResetInfo').html('Your enquiry has been sent successfully!');
                           $('#divMsg').fadeIn('slow');
                           $('#txt_names, #txt_email, #txt_message').val('');
                           setTimeout(function () {
                               $('#divMsg').fadeOut();
                               $('#msgResetInfo').html('');
                           }, 10000);
                           $.unblockUI();
                           e.preventDefault();
                           return;
                           //var $form = $(e.target);
                       }
                   });
                   e.preventDefault();

               }
           });
});




function facebookSignup() {
    if (jQuery("#invitationCode").val() === "") {
        $("#error").remove();
        $("#alert").append('<span id="error"> Please enter your invitation code.</span>');
        $('#alert').fadeIn('slow');
        $.unblockUI();
        return;
    }
    var model = {
        InvitationCode: jQuery("#invitationCode").val()
    };
    $.blockUI({
        baseZ: 30009
    });
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/User/FacebookSignup",
        contentType: "application/json"
    }).done(function (res) {
        if (res === "Invitation Code you entered is invalid.") {
            $("#error").remove();
            $("#alert").append('<span id="error"> The invitation code you entered is invalid.</span>');
            $('#alert').fadeIn('slow');
            $.unblockUI();
            document.location.href = "#alert";
            return;

        } else if (res === "Email address you entered is not available. Please try another.") {
            $("#error").remove();
            $("#alert").append('<span id="error"> The email address you entered is already in use. Please try another.</span>');
            $('#alert').fadeIn('slow');
            $.unblockUI();
            document.location.href = "#alert";
            return;

        } else if (res === "Invitation Code you entered is already used.") {
            $("#error").remove();
            $("#alert").append('<span id="error"> The invitation code you entered is already in use.</span>');
            $('#alert').fadeIn('slow');
            $.unblockUI();
            document.location.href = "#alert";
            return;

        } else if (res === "success") {
            $("#error").remove();
            $.unblockUI();
        }
    });
};
function facebookLogin() {
    var model = {
        InvitationCode: ""
    };
    $.blockUI({
        baseZ: 30009
    });
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/User/FacebookSignup",
        contentType: "application/json"
    }).done(function (res) {
        if (res === "Invitation Code you entered is invalid.") {
            $.unblockUI();
        }
    });
};
function twitterSignup() {
    if (jQuery("#invitationCode").val() === "") {
        $("#error").remove();
        $("#alert").append('<span id="error"> Please enter your invitation code.</span>');
        $('#alert').fadeIn('slow');
        $.unblockUI();
        return;
    }
    var model = {
        InvitationCode: jQuery("#invitationCode").val()
    };
    $.blockUI({
        baseZ: 30009
    });
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/User/TwitterSignupBegin",
        contentType: "application/json"
    }).done(function (res) {
        if (res === "Invitation Code you entered is invalid.") {
            $("#error").remove();
            $("#alert").append('<span id="error"> The invitation code you entered is invalid.</span>');
            $('#alert').fadeIn('slow');
            $.unblockUI();
            document.location.href = "#alert";
            return;

        } else if (res === "Email address you entered is not available. Please try another.") {
            $("#error").remove();
            $("#alert").append('<span id="error"> The email address you entered is already in use. Please try another.</span>');
            $('#alert').fadeIn('slow');
            $.unblockUI();
            document.location.href = "#alert";
            return;

        } else if (res === "Invitation Code you entered is already used.") {
            $("#error").remove();
            $("#alert").append('<span id="error"> The invitation code you entered is already in use.</span>');
            $('#alert').fadeIn('slow');
            $.unblockUI();
            document.location.href = "#alert";
            return;

        } else if (res === "success") {
            $("#error").remove();
            $.unblockUI();
        }
    });
};

function twitterLogin() {
    var model = {
        InvitationCode: ""
    };
    $.blockUI({
        baseZ: 30009
    });
    $.ajax({
        type: "POST",
        data: JSON.stringify(model),
        url: "/User/TwitterSignupBegin",
        contentType: "application/json"
    }).done(function (res) {
        $.unblockUI();
    });
};

///////////HealthGoal/////////////////
function SetWeightGoal() {
    var myWindow = $("#window"),
    undo = $("#lnkSetWeightGoal"),
    btnSetWeight = $("#btnSetWeightGoal");

    btnSetWeight.click(function () {
        var goal = Number($("#txtWeightGoal").val());
        if (goal > 0) {
            $.ajax({
                type: "POST",
                data: JSON.stringify({ goal: goal }),
                url: "/User/SetWeightGoal",
                contentType: "application/json",
                dataType: 'json'
            })
                .done(function (data) {
                    if (data === "Goal set") {
                        myWindow.data("kendoWindow").close();
                        undo.fadeIn();
                        $('#goalWeight').text(goal + " lbs");
                        createWeightChart();
                    }
                });
        }
    });

    undo.click(function () {
        myWindow.data("kendoWindow").open();
        undo.fadeOut();
    });

    function onClose() {
        undo.fadeIn();
    }

    myWindow.kendoWindow({
        width: "200px",
        height: "120px",
        title: "Set Goal (lbs)",
        resizable: false,
        visible: false,
        modal: true,
        actions: [
            "Close"
        ],
        close: onClose
    }).data("kendoWindow").center();
};

function SetDietGoal() {
    var myWindow = $("#windowDiet"),
    undo = $("#lnkSetDietGoal"),
    btnSetDietGoal = $("#btnSetDietGoal");
    
    btnSetDietGoal.click(function () {
        var goal = Number($("#txtDietGoal").val());
        if (goal > 0) {
            $.ajax({
                type: "POST",
                data: JSON.stringify({ goal: goal }),
                url: "/User/SetDietGoal",
                contentType: "application/json",
                dataType: 'json'
            })
                .done(function (data) {
                    if (data === "Goal set") {
                        myWindow.data("kendoWindow").close();
                        undo.fadeIn();
                        $('#goalDiet').text(goal + " Kcals");
                        createDietChart();
                    }
                });
        }
    });

    undo.click(function () {
        myWindow.data("kendoWindow").open();
        undo.fadeOut();
    });

    function onClose() {
        undo.fadeIn();
    }

    myWindow.kendoWindow({
        width: "200px",
        height: "120px",
        title: "Set Goal (Kcals)",
        resizable: false,
        visible: false,
        modal: true,
        actions: [
            "Close"
        ],
        close: onClose
    }).data("kendoWindow").center();
};

function SetExerciseGoal() {
    var myWindow = $("#windowExercise"),
    undo = $("#lnkSetExerciseGoal"),
    btnSetExerciseGoal = $("#btnSetExerciseGoal");

    btnSetExerciseGoal.click(function () {
        var goal = Number($("#txtExerciseGoal").val());
        if (goal > 0) {
            $.ajax({
                type: "POST",
                data: JSON.stringify({ goal: goal }),
                url: "/User/SetExerciseGoal",
                contentType: "application/json",
                dataType: 'json'
            })
                .done(function (data) {
                    if (data === "Goal set") {
                        myWindow.data("kendoWindow").close();
                        undo.fadeIn();
                        $('#goalDuration').text(goal + " hrs");
                      //  GetExerciseGoal($('#txtDateRangeFilter').data('daterangepicker').startDate, $('#txtDateRangeFilter').data('daterangepicker').endDate);
                        createExerciseChart();
                    }
                });
        }
    });

    undo.click(function () {
        myWindow.data("kendoWindow").open();
        undo.fadeOut();
    });

    function onClose() {
        undo.fadeIn();
    }

    myWindow.kendoWindow({
        width: "200px",
        height: "120px",
        title: "Set Goal (hrs)",
        resizable: false,
        visible: false,
        modal: true,
        actions: [
            "Close"
        ],
        close: onClose
    }).data("kendoWindow").center();
};

function SetSleepGoal() {
    var myWindow = $("#windowSleep"),
    undo = $("#lnkSetSleepGoal"),
    btnSetSleepGoal = $("#btnSetSleepGoal");

    btnSetSleepGoal.click(function () {
        var goal = Number($("#txtSleepGoal").val());
        if (goal > 0) {
            $.ajax({
                type: "POST",
                data: JSON.stringify({ goal: goal }),
                url: "/User/SetSleepGoal",
                contentType: "application/json",
                dataType: 'json'
            })
                .done(function (data) {
                    if (data === "Goal set") {
                        myWindow.data("kendoWindow").close();
                        undo.fadeIn();
                        $('#goalSleep').text(goal + " mins");
                        //GetSleepGoal($('#txtDateRangeFilter').data('daterangepicker').startDate, $('#txtDateRangeFilter').data('daterangepicker').endDate);
                        createSleepChart();
                    }
                });
        }
    });

    undo.click(function () {
        myWindow.data("kendoWindow").open();
        undo.fadeOut();
    });

    function onClose() {
        undo.fadeIn();
    }

    myWindow.kendoWindow({
        width: "200px",
        height: "120px",
        title: "Set Goal (mins)",
        resizable: false,
        visible: false,
        modal: true,
        actions: [
            "Close"
        ],
        close: onClose
    }).data("kendoWindow").center();
};

function GetWeightGoal(start, end) {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: start,
            endDate: end
        }),
        url: "/User/WeightChart",
        contentType: "application/json",
        dataType: 'json'
    })
           .done(function (data) {
               var arrayLength = data.userVital.length;
               for (var i = 0; i < arrayLength; i++) {
                   if (data.userVital[i].WeightLbs > weightHGMax) {
                       weightHGMax = data.userVital[i].WeightLbs + (data.userVital[i].WeightLbs * .25);
                   }
                   if (data.userVital[i].WeightLbs < weightHGMin) {
                       weightHGMin = data.userVital[i].WeightLbs - (data.userVital[i].WeightLbs * .15);
                   }
               }

               var chart = $("#chartWeight").data("kendoChart")
               var options = {
                   valueAxis: {
                       max: weightHGMax,
                       min: weightHGMin
                   }
               };
               chart.setOptions(options);
               chart.dataSource.data(data.userVital);
           });
};
function createWeightChart() {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null
        }),
        url: "/User/Last5GoalsWeight",
        contentType: "application/json",
        dataType: 'json'
    })
        .done(function (data) {
            var plotbandloaded = false;
            $('#currentWeight').text(Math.round(data.userGoalDetails.CurrentWeightLbs * 100) / 100 + " lbs");            
            $('#goalWeight').text(data.userGoalDetails.Goal + " lbs");
            if (data.userGoalDetails.Goal != null) {
                weightHGoal = data.userGoalDetails.Goal;
                weightHgoalTo = weightHGoal + (weightHGoal * .025);
                weightHGMax = weightHGoal + (weightHgoalTo * .25);
                weightHGMin = weightHGoal - (weightHGoal * .15);
                var arrayLength = data.userVital.length;
                for (var i = 0; i < arrayLength; i++) {
                    if (data.userVital[i].WeightLbs > weightHGMax) {
                        weightHGMax = data.userVital[i].WeightLbs + (data.userVital[i].WeightLbs * .25);
                    }
                    if (data.userVital[i].WeightLbs < weightHGMin) {
                        weightHGMin = data.userVital[i].WeightLbs - (data.userVital[i].WeightLbs * .15);
                    }
                }
            }

            $('#chartWeight').kendoChart({
                dataSource: {
                    data: data.userVital,
                    sort: {
                        field: "ResultDateTime",
                        dir: "asc"
                    }
                },
                legend: {
                    position: "bottom"
                },
                chartArea: {
                    background: "",
                    width: 300,
                    height: 190
                },
                seriesDefaults: {
                    type: "line",
                    style: "smooth"
                },
                series: [{
                    field: "WeightLbs",
                    name: "Weight (lbs)",
                    color: "#00B5A3"
                }],
                valueAxis: {
                    max: weightHGMax,
                    min: weightHGMin,
                    labels: {
                        format: "{0}"
                    },
                    name: "valueAxis",
                    line: {
                        visible: false
                    }
                },
                tooltip: {
                    visible: true,
                    format: "{0}%",
                    template: "#= series.name #: #= value # <br> Date: #= kendo.toString(category, 'M/d/yy') #"
                },
                categoryAxis: {
                    field: "Day",
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    }
                },
                dataBound: function(e) {
                    setTimeout(function () {
                        if (plotbandloaded == false && data.userGoalDetails.Goal != 0) {                           
                            var chart = $('#chartWeight').data("kendoChart");
                            var options = {
                                valueAxis: {
                                    max: weightHGMax,
                                    min: weightHGMin,
                                    plotBands: [{
                                        from: weightHGoal,
                                        to: weightHgoalTo,
                                        color: "#c00",
                                        opacity: 0.3
                                    }]
                                }
                            };
                            plotbandloaded = true;
                            chart.setOptions(options);
                        };
                    }, 100)
                }
            });
        });
};

function GetExerciseGoal(start, end) {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: start,
            endDate: end
        }),
        url: "/User/ExerciseChart",
        contentType: "application/json",
        dataType: 'json'
    })
           .done(function (data) {
               var arrayLength = data.userActivity.length;
               for (var i = 0; i < arrayLength; i++) {
                   if (data.userActivity[i].DurationMinutes > excHGMax) {
                       excHGMax = data.userActivity[i].DurationMinutes + (data.userActivity[i].DurationMinutes * .25);
                   }
                   if (data.userActivity[i].DurationMinutes < excHGMin) {
                       excHGMin = data.userActivity[i].DurationMinutes - (data.userActivity[i].DurationMinutes * .15);
                   }
               }

               var chart = $("#chartExercise").data("kendoChart")
               var options = {
                   valueAxis: {
                       max: excHGMax,
                       min: excHGMin
                   }
               };
               chart.setOptions(options);
               chart.dataSource.data(data.userActivity);
           });
};

excerciseChartDataUpdated = false;
axisCrossingUpdated = false;

function createExerciseChart() {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null
        }),
        url: "/User/Last5GoalsExercise",
        contentType: "application/json",
        dataType: 'json'
    })
        .done(function (data) {
            var plotbandloaded = false;
            var axisCrossing1 = data.userActivity.length + 1;
            $('#currentDuration').text(Math.round(data.userGoalDetails.DurationHours * 100) / 100 + " hrs");
            $('#goalDuration').text(Math.round(data.userGoalDetails.Goal * 100) / 100 + " hrs");
            if (data.userGoalDetails.Goal != null) {
                excHGoal = data.userGoalDetails.Goal;
                excHgoalTo = excHGoal + (excHGoal * .025);
                excHGMax = excHGoal + (excHgoalTo * .25);
                excHGMin = excHGoal - (excHGoal * .15);
                var arrayLength = data.userActivity.length;
                for (var i = 0; i < arrayLength; i++) {
                    if (data.userActivity[i].DurationMinutes > excHGMax) {
                        excHGMax = data.userActivity[i].DurationMinutes + (data.userActivity[i].DurationMinutes * .25);
                    }
                    if (data.userActivity[i].DurationMinutes < excHGMin) {
                        excHGMin = data.userActivity[i].DurationMinutes - (data.userActivity[i].DurationMinutes * .15);
                    }
                }
            }

            $("#chartExercise").kendoChart(
            {
                dataSource: {
                    data: data.userActivity,
                    sort: {
                        field: "StartDateTime",
                        dir: "asc"
                    }
                },
                title: {
                    text: ""
                },
                legend: {
                    position: "bottom"
                },
                chartArea: {
                    background: "",
                    width: 300,
                    height: 190
                },
                seriesDefaults: {
                    type: "line",
                    style: "smooth"
                },
                series: [
                    {
                        field: "DurationMinutes",
                        name: "Minutes",
                        axis: "DurationMinutes",
                        color: "#4D4D4D",
                        type: "column"
                    }, {
                        name: "Calories (Kcals)",
                        field: "Calories",
                        axis: "Calories",
                        color: "#00B5A3",
                        type: "line"
                    }
                ],
                valueAxes: [{
                    max: excHGMax,
                    min: excHGMin,
                    name: "DurationMinutes",
                    color: "#4D4D4D"
                }, {
                    name: "Calories",
                    color: "#00B5A3"
                }],
                categoryAxis: {
                    field: "Day",
                    majorGridLines: {
                        visible: false
                    },
                    axisCrossingValues: [0, axisCrossing1],
                    labels: {
                        rotation: "auto"
                    }
                },
                tooltip: {
                    visible: true,
                    format: "{0}%",
                    template: "#= series.name #: #= value # <br> Date: #= kendo.toString(category, 'M/d/yy') #"
                },
                dataBound: function (e) {
                    setTimeout(function () {
                        if (excerciseChartDataUpdated == true && axisCrossingUpdated == false) {
                            var chart = $('#chartExercise').data("kendoChart");
                            var axisCrossing2 = chart.options.series[1].data.length + 1;
                            var options = {
                                valueAxes: [{
                                    max: excHGMax,
                                    min: excHGMin,
                                    name: "DurationMinutes",
                                    color: "#4D4D4D",                                    
                                }, {
                                    name: "Calories",
                                    color: "#00B5A3"
                                }],
                                categoryAxis: { axisCrossingValues: [0, axisCrossing2] }
                            };
                            excerciseChartDataUpdated == false;
                            axisCrossingUpdated = true;
                            chart.setOptions(options);
                        }
                        if (plotbandloaded == false && data.userGoalDetails.Goal != 0) {
                            var chart = $('#chartExercise').data("kendoChart");
                            var axisCrossing2 = chart.options.series[1].data.length + 1;
                            var options = {
                                valueAxes: [{
                                        name: "DurationMinutes",
                                        color: "#4D4D4D",
                                        max: excHGMax,
                                        min: excHGMin,
                                        plotBands: [{
                                            from: excHGoal,
                                            to: excHgoalTo,
                                            color: "#c00",
                                            opacity: 0.3
                                        }]
                                    }, {
                                        name: "Calories",
                                        color: "#00B5A3"
                                    }],
                                categoryAxis: { axisCrossingValues: [0, axisCrossing2] }
                                };

                            plotbandloaded = true;
                            chart.setOptions(options);
                        };
                    }, 100)
                }
            });

        });
};


function GetDietGoal(start, end) {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: start,
            endDate: end
        }),
        url: "/User/DietChart",
        contentType: "application/json",
        dataType: 'json'
    })
           .done(function (data) {
               var arrayLength = data.userDiet.length;
               for (var i = 0; i < arrayLength; i++) {
                   if (data.userDiet[i].Value > dietHGMax) {
                       dietHGMax = data.userDiet[i].Value + (data.userDiet[i].Value * .25);
                   }
                   if (data.userDiet[i].Value < dietHGMin) {
                       dietHGMin = data.userDiet[i].Value - (data.userDiet[i].Value * .15);
                   }
               }
               var chart = $("#chartDiet").data("kendoChart")
               var options = {
                   valueAxis: {
                       max: dietHGMax,
                       min: dietHGMin
                   }
               };
               chart.setOptions(options);
               chart.dataSource.data(data.userDiet);
           });
};
function createDietChart() {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null
        }),
        url: "/User/Last5GoalsDiet",
        contentType: "application/json",
        dataType: 'json'
    })
        .done(function (data) {
            var plotbandloaded = false;
            $('#currentDiet').text(data.userGoalDetails.Value + " Kcals");
            $('#goalDiet').text(data.userGoalDetails.Goal + " Kcals");
            if (data.userGoalDetails.Goal != null) {
                diettHGoal = data.userGoalDetails.Goal;
                dietHgoalTo = diettHGoal + (diettHGoal * .025);
                dietHGMax = diettHGoal + (dietHgoalTo * .25);
                dietHGMin = diettHGoal - (diettHGoal * .25);
                var arrayLength = data.userDiet.length;
                for (var i = 0; i < arrayLength; i++) {
                    if (data.userDiet[i].Value > dietHGMax) {
                        dietHGMax = data.userDiet[i].Value + (data.userDiet[i].Value * .25);
                    }
                    if (data.userDiet[i].Value < dietHGMin) {
                        dietHGMin = data.userDiet[i].Value - (data.userDiet[i].Value * .15);
                    }
                }
            }

            $('#chartDiet').kendoChart({
                dataSource: {
                    data: data.userDiet,
                    sort: {
                        field: "EnteredDateTime",
                        dir: "asc"
                    }
                },
                legend: {
                    position: "bottom"
                },
                chartArea: {
                    background: "",
                    width: 300,
                    height: 190
                },
                seriesDefaults: {
                    type: "line",
                    style: "smooth"
                },
                series: [{
                    field: "Value",
                    name: "Calories (Kcals)",
                    color: "#00B5A3"
                }],
                valueAxis: {
                    max: dietHGMax,
                    min: dietHGMin,
                    labels: {
                        format: "{0}"
                    },
                    line: {
                        visible: false
                    }
                },
                tooltip: {
                    visible: true,
                    format: "{0}%",
                    template: "#= series.name #: #= value # <br> Date: #= kendo.toString(category, 'M/d/yy') #"
                },
                categoryAxis: {
                    field: "Day",
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    }
                },
                dataBound: function (e) {
                    setTimeout(function () {
                        if (plotbandloaded == false && data.userGoalDetails.Goal != 0) {
                            var chart = $('#chartDiet').data("kendoChart");
                            var options = {
                                valueAxis: {
                                    max: dietHGMax,
                                    min: dietHGMin,
                                    plotBands: [{
                                        from: diettHGoal,
                                        to: dietHgoalTo,
                                        color: "#c00",
                                        opacity: 0.3
                                    }]
                                }
                            };
                            plotbandloaded = true;
                            chart.setOptions(options);
                        };
                    }, 100)
                }
            });

        });
};

function GetSleepGoal(start, end) {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: start,
            endDate: end
        }),
        url: "/User/SleepChart",
        contentType: "application/json",
        dataType: 'json'
    })
           .done(function (data) {
               var arrayLength = data.userSleep.length;
               for (var i = 0; i < arrayLength; i++) {
                   if (data.userSleep[i].TimeAsleep > sleepHGMax) {
                       sleepHGMax = data.userSleep[i].TimeAsleep + (data.userSleep[i].TimeAsleep * .25);
                   }
                   if (data.userSleep[i].TimeAsleep < sleepHGMin) {
                       sleepHGMin = data.userSleep[i].TimeAsleep - (data.userSleep[i].TimeAsleep * .15);
                   }
               }
               var chart = $('#chartSleep').data("kendoChart");
               var options = {
                   valueAxis: {
                       max: sleepHGMax,
                       min: sleepHGMin                       
                   }
               };
               chart.setOptions(options);
               chart.dataSource.data(data.userSleep);
           });
};
function createSleepChart() {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null
        }),
        url: "/User/Last5GoalsSleep",
        contentType: "application/json",
        dataType: 'json'
    })
        .done(function (data) {
            var plotbandloaded = false;            
            $('#currentSleep').text(data.userGoalDetails.TimeAsleep + " mins");
            $('#goalSleep').text(data.userGoalDetails.Goal + " mins");
            if (data.userGoalDetails.Goal != null) {
                sleepHGoal = data.userGoalDetails.Goal;
                sleepHgoalTo = sleepHGoal + (sleepHGoal * .025);
                sleepHGMax = sleepHGoal + (sleepHgoalTo * .25);
                sleepHGMin = sleepHGoal - (sleepHGoal * .15);
                var arrayLength = data.userSleep.length;
                for (var i = 0; i < arrayLength; i++) {
                    if (data.userSleep[i].TimeAsleep > sleepHGMax) {
                        sleepHGMax = data.userSleep[i].TimeAsleep + (data.userSleep[i].TimeAsleep * .25);
                    }
                    if (data.userSleep[i].TimeAsleep < sleepHGMin) {
                        sleepHGMin = data.userSleep[i].TimeAsleep - (data.userSleep[i].TimeAsleep * .15);
                    }
                }
            }
            $('#chartSleep').kendoChart({
                dataSource: {
                    data: data.userSleep,
                    sort: {
                        field: "StartDateTime",
                        dir: "asc"
                    }
                },
                legend: {
                    position: "bottom"
                },
                chartArea: {
                    background: "",
                    width: 300,
                    height: 190
                },
                seriesDefaults: {
                    type: "column",
                    style: "smooth"
                },
                series: [{
                    field: "TimeAsleep",
                    name: "Sleep (Minutes)",
                    color: "#00B5A3"
                }],
                valueAxis: {
                    max: sleepHGMax,
                    min: sleepHGMin,
                    labels: {
                        format: "{0}"
                    },                    
                    line: {
                        visible: false
                    }
                },
                tooltip: {
                    visible: true,
                    format: "{0}%",
                    template: "#= series.name #: #= value # <br> Date: #= kendo.toString(category, 'M/d/yy') #"
                },
                categoryAxis: {
                    field: "Day",
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    }
                },
                dataBound: function (e) {
                    setTimeout(function () {
                        if (plotbandloaded == false && data.userGoalDetails.Goal != 0) {
                            var chart = $('#chartSleep').data("kendoChart");
                            var options = {
                                valueAxis: {
                                    max: sleepHGMax,
                                    min: sleepHGMin,
                                    plotBands: [{
                                        from: sleepHGoal,
                                        to: sleepHgoalTo,
                                        color: "#c00",
                                        opacity: 0.3
                                    }]
                                }
                            };
                            plotbandloaded = true;
                            chart.setOptions(options);
                        };
                    }, 100)
                }
            });
        });
};



function GetBloodGlucoseChart(start, end) {
        $.ajax({
            type: "POST",
            data: JSON.stringify({
                startDate: start,
                endDate: end
            }),
            url: "/User/BloodGlucoseChart",
            contentType: "application/json",
            dataType: 'json'
        })
          .done(function (data) {
              if (data.ResultDateTime != null) {
                  $('#bgDate').text(moment(data.ResultDateTime).format("MM/DD/YYYY"));
              } else {
                  $('#bgDate').text("");
              }
              $("#chartBloodGlucose").data("kendoChart").dataSource.data(data.userBloodGlucose);
              $('#currentBloodGlucose').text(data.Value);
          });
};
function createBloodGlucoseChart() {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null,
            Last5: true
        }),
        url: "/User/BloodGlucoseChart",
        contentType: "application/json",
        dataType: 'json'
    })
        .done(function (data) {
            $('#currentBloodGlucose').text(data.Value);
            if (data.ResultDateTime != null) {
                $('#bgDate').text(moment(data.ResultDateTime).format("MM/DD/YYYY"));
            } else {
                $('#bgDate').text("");
            }

            $('#chartBloodGlucose').kendoChart({
                dataSource: {
                    data: data.userBloodGlucose,
                    sort: {
                        field: "ResultDateTime",
                        dir: "asc"
                    }
                },
                legend: {
                    visible:false
                },
                chartArea: {
                    background: "",
                    width: 130,
                    height: 90
                },
                seriesDefaults: {
                    type: "line",
                    style: "smooth"
                },
                series: [{
                    field: "Value",
                    name: "Blood Glucose",
                    color: "#00B5A3"
                }],
                valueAxis: {
                    min: 60,
                    max: 200,
                    line: {
                        visible: false
                    },
                    visible: false
                },
                tooltip: {
                    visible: true,
                    format: "{0}%",
                    template: "#= series.name #: #= value # <br> Date: #= kendo.toString(category, 'M/d/yy') #"
                },
                categoryAxis: {
                    field: "Day",
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    },
                    visible: false
                }
            });
        });
};


function GetBloodPressureChart(start, end) {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: start,
            endDate: end
        }),
        url: "/User/BloodPressureChart",
        contentType: "application/json",
        dataType: 'json'
    })
      .done(function (data) {
          $("#chartBloodPressure").data("kendoChart").dataSource.data(data.userBloodPressure);
          $('#bloodPressure').text(data.Systolic + "/" + data.Diastolic);
          $('#heartRate').text(data.HeartRate);

          if (data.ResultDateTime != null) {
              $('#bpDate').text(moment(data.ResultDateTime).format("MM/DD/YYYY"));
          } else {
              $('#bpDate').text("");
          }
      });
};

function createBloodPressureChart() {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null,
            Last5: true
        }),
        url: "/User/BloodPressureChart",
        contentType: "application/json",
        dataType: 'json'
    })
        .done(function (data) {
            $('#bloodPressure').text(data.Systolic + "/" + data.Diastolic);
            $('#heartRate').text(data.HeartRate);

            if (data.ResultDateTime != null) {
                $('#bpDate').text(moment(data.ResultDateTime).format("MM/DD/YYYY"));
            } else {
                $('#bpDate').text("");
            }

            $('#chartBloodPressure').kendoChart({
                dataSource: {
                    data: data.userBloodPressure,
                    sort: {
                        field: "ResultDateTime",
                        dir: "asc"
                    }
                },
                legend: {
                    visible: false
                },
                chartArea: {
                    background: "",
                    width: 130,
                    height: 90
                },
                seriesDefaults: {
                    type: "line",
                    style: "smooth"
                },
                series: [{
                    field: "Systolic",
                    name:"Systolic",
                    color: "#00B5A3",
                    type: "line"
                },  {
                    field: "Diastolic",
                    name:"Diastolic",
                    color: "#4D4D4D",
                    type: "line"
                }],
                valueAxis: {
                    min: 60,
                    max: 250,
                    line: {
                        visible: false
                    },
                    visible: false
                },
                tooltip: {
                    visible: true,
                    format: "{0}%",
                    template: "#= series.name #: #= value # <br> Date: #= kendo.toString(category, 'M/d/yy') #"
                },
                categoryAxis: {
                    field: "Day",
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    },
                    visible: false
                }
            });
        });
};


function GetStepsChart(start, end) {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: start,
            endDate: end
        }),
        url: "/User/StepsChart",
        contentType: "application/json",
        dataType: 'json'
    })
      .done(function (data) {
          $("#chartSteps").data("kendoChart").dataSource.data(data.userActivity);
          $('#steps').text(data.Steps);
          if (data.StartDateTime != null) {
              $('#stepsDate').text(moment(data.StartDateTime).format("MM/DD/YYYY"));
          } else {
              $('#stepsDate').text("");
          }
      });
};
function createStepsChart() {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null,
            Last5: true
        }),
        url: "/User/StepsChart",
        contentType: "application/json",
        dataType: 'json'
    })
        .done(function (data) {
            $('#steps').text(data.Steps);

            if (data.StartDateTime != null) {
                $('#stepsDate').text(moment(data.StartDateTime).format("MM/DD/YYYY"));
            } else {
                $('#stepsDate').text("");
            }
         
            $('#chartSteps').kendoChart({
                dataSource: {
                    data: data.userActivity,
                    sort: {
                        field: "StartDateTime",
                        dir: "asc"
                    }
                },
                legend: {
                    visible: false
                },
                chartArea: {
                    background: "",
                    width: 130,
                    height: 90
                },
                seriesDefaults: {
                    type: "column",
                    style: "smooth"
                },
                series: [{
                    field: "Steps",
                    name: "Steps",
                    color: "#00B5A3"
                }],
                valueAxis: {
                    line: {
                        visible: false
                    },
                    visible: false
                },
                tooltip: {
                    visible: true,
                    format: "{0}%",
                    template: "#= series.name #: #= value # <br> Date: #= kendo.toString(category, 'M/d/yy') #"
                }
                ,
                categoryAxis: {
                    field: "Day",
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    },
                    visible: false
                }
            });
        });
};

function GetWeightTrendChart(start, end) {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: start,
            endDate: end
        }),
        url: "/User/WeightTrendChart",
        contentType: "application/json",
        dataType: 'json'
    })
      .done(function (data) {
          $("#chartWeightTrend").data("kendoChart").dataSource.data(data.userVital);
          $('#weight').text(Math.round(data.WeightLbs * 100) / 100);
          if (data.ResultDateTime != null) {
              $('#weightResultDate').text(moment(data.ResultDateTime).format("MM/DD/YYYY"));
          } else {
              $('#weightResultDate').text("");
          }


      });
};
function createWeightTrendChart() {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null,
            Last5: true
        }),
        url: "/User/WeightTrendChart",
        contentType: "application/json",
        dataType: 'json'
    })
        .done(function (data) {
            $('#weight').text(Math.round(data.WeightLbs * 100) / 100);
            if (data.ResultDateTime != null) $('#weightResultDate').text(moment(data.ResultDateTime).format("MM/DD/YYYY"));
           
            $('#chartWeightTrend').kendoChart({
                dataSource: {
                    data: data.userVital,
                    sort: {
                        field: "ResultDateTime",
                        dir: "asc"
                    }
                },
                legend: {
                    visible: false
                },
                chartArea: {
                    background: "",
                    width: 130,
                    height: 90
                },
                seriesDefaults: {
                    type: "line",
                    style: "smooth"
                },
                series: [{
                    field: "WeightLbs",
                    name: "Weight (lbs)",
                    color: "#00B5A3"
                }],
                valueAxis: {
                    line: {
                        visible: false
                    },
                    visible: false
                },
                tooltip: {
                    visible: true,
                    format: "{0}%",
                    template: "#= series.name #: #= value # <br> Date: #= kendo.toString(category, 'M/d/yy') #"
                }
                ,
                categoryAxis: {
                    field: "Day",
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    },
                    visible: false
                }
            });
        });
};


function GetDietTrendChart(start, end) {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: start,
            endDate: end
        }),
        url: "/User/DietTrendChart",
        contentType: "application/json",
        dataType: 'json'
    })
      .done(function (data) {
          $("#chartDietTrend").data("kendoChart").dataSource.data(data.userDiet);
          $('#diet').text(data.Value);
          if (data.EnteredDateTime != null) {
              $('#dietDate').text(moment(data.EnteredDateTime).format("MM/DD/YYYY"));
          } else {
              $('#dietDate').text("");
          }
      });
};
function createDietTrendChart() {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null,
            Last5: true
        }),
        url: "/User/DietTrendChart",
        contentType: "application/json",
        dataType: 'json'
    })
        .done(function (data) {
            $('#diet').text(data.Value);

            if (data.EnteredDateTime != null) {
                $('#dietDate').text(moment(data.EnteredDateTime).format("MM/DD/YYYY"));
            } else {
                $('#dietDate').text("");
            }
            $('#chartDietTrend').kendoChart({
                dataSource: {
                    data: data.userDiet,
                    sort: {
                        field: "EnteredDateTime",
                        dir: "asc"
                    }
                },
                legend: {
                    visible: false
                },
                chartArea: {
                    background: "",
                    width: 130,
                    height: 90
                },
                seriesDefaults: {
                    type: "column",
                    style: "smooth"
                },
                series: [{
                    field: "Value",
                    name: "Calories (Kcals)",
                    color: "#00B5A3"
                }],
                valueAxis: {
                    line: {
                        visible: false
                    },
                    visible: false
                },
                tooltip: {
                    visible: true,
                    format: "{0}%",
                    template: "#= series.name #: #= value # <br> Date: #= kendo.toString(category, 'M/d/yy') #"
                }
                ,
                categoryAxis: {
                    field: "Day",
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    },
                    visible: false
                }
            });
        });
};


function GetBodyCompositionTrendChart(start, end) {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: start,
            endDate: end
        }),
        url: "/User/BodyCompositionChart",
        contentType: "application/json",
        dataType: 'json'
    })
      .done(function (data) {
          $("#chartBodyCompositionTrend").data("kendoChart").dataSource.data(data.userBodyComposition);
          $('#bodyFat').text(data.userBodyComposition[0].Value);
          $('#leanTissue').text(data.userBodyComposition[1].Value);
          $('#bodyDate').text(moment(data.ResultDateTime).format("MM/DD/YYYY"));

      });
};
function createBodyCompositionTrendChart() {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null,
            Last5: true
        }),
        url: "/User/BodyCompositionChart",
        contentType: "application/json",
        dataType: 'json'
    })
        .done(function (data) {
            $('#bodyFat').text(data.userBodyComposition[0].Value);
            $('#leanTissue').text(data.userBodyComposition[1].Value);
            $('#bodyDate').text(moment(data.ResultDateTime).format("MM/DD/YYYY"));

            $('#chartBodyCompositionTrend').kendoChart({
                dataSource: {
                    data: data.userBodyComposition
                },
               legend: {
                    visible: false
                },
                chartArea: {
                    background: "",
                    width: 130,
                    height: 90
                },
                seriesDefaults: {
                    type: "pie",
                    style: "smooth"
                },
                series: [{
                    type: "pie",
                    field: "Value",
                    categoryField: "Source"
                }],
    seriesColors: ["#00B5A3", "#4D4D4D"]
            });
        });
};

function GetCholesterolChart(start, end) {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: start,
            endDate: end
        }),
        url: "/User/CholestrolChart",
        contentType: "application/json",
        dataType: 'json'
    })
      .done(function (data) {
          $("#chartCholesterol").data("kendoChart").dataSource.data(data.userTest);
          $('#ldl').text(data.userResult.Ldl);
          $('#hdl').text(data.userResult.Hdl);
          $('#cholesterol').text(data.userResult.Cholesterol);
          $('#dateCholesterol').text(moment(data.userResult.ResultDateTime).format("MM/DD/YYYY"));

      });
};
function createCholesterolChart() {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null,
            Last5: true
        }),
        url: "/User/CholestrolChart",
        contentType: "application/json",
        dataType: 'json'
    })
        .done(function (data) {
            if (data.userResult != null)
            {
                $('#ldl').text(data.userResult.Ldl);
                $('#hdl').text(data.userResult.Hdl);
                $('#cholesterol').text(data.userResult.Cholesterol);
                $('#dateCholesterol').text(moment(data.userResult.ResultDateTime).format("MM/DD/YYYY"));

            }
            
            $('#chartCholesterol').kendoChart({
                dataSource: {
                    data: data.userTest,
                    sort: {
                        field: "ResultDateTime",
                        dir: "asc"
                    }
                },
                legend: {
                    visible: false
                },
                chartArea: {
                    background: "",
                    width: 130,
                    height: 90
                },
                seriesDefaults: {
                    type: "line",
                    style: "smooth"
                },
                series: [{
                    field: "Ldl",
                    name: "Ldl",
                    color: "#00B5A3",
                    type: "line"
                }, {
                    field: "Hdl",
                    name: "Hdl",
                    color: "#4D4D4D",
                    type: "line"
                }],
                valueAxis: {
                    min: 20,
                    max: 250,
                    line: {
                        visible: false
                    },
                    visible: false
                },
                tooltip: {
                    visible: true,
                    format: "{0}%",
                    template: "#= series.name #: #= value # <br> Date: #= kendo.toString(category, 'M/d/yy') #"
                },
                categoryAxis: {
                    field: "Day",
                    majorGridLines: {
                        visible: false
                    },
                    labels: {
                        rotation: "auto"
                    },
                    visible: false
                }
            });
        });
};

function GetGoalLast5() {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null
        }),
        url: "/User/Last5GoalsWeight",
        contentType: "application/json",
        dataType: 'json'
    })
           .done(function (data) {
               $("#chartWeight").data("kendoChart").dataSource.data(data.userVital);
           });

    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null
        }),
        url: "/User/Last5GoalsDiet",
        contentType: "application/json",
        dataType: 'json'
    })
       .done(function (data) {
           $("#chartDiet").data("kendoChart").dataSource.data(data.userDiet);
       });
    
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null
        }),
        url: "/User/Last5GoalsSleep",
        contentType: "application/json",
        dataType: 'json'
    })
       .done(function (data) {
           $("#chartSleep").data("kendoChart").dataSource.data(data.userSleep);
       });
    
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null
        }),
        url: "/User/Last5GoalsExercise",
        contentType: "application/json",
        dataType: 'json'
    })
       .done(function (data) {
           excerciseChartDataUpdated == true;
           $("#chartExercise").data("kendoChart").dataSource.data(data.userActivity);
       });
};

function GetTrendsLast5() {
    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null,
            Last5: true
        }),
        url: "/User/BloodGlucoseChart",
        contentType: "application/json",
        dataType: 'json'
    })
          .done(function (data) {
              if (data.ResultDateTime != null) {
                  $('#bgDate').text(moment(data.ResultDateTime).format("MM/DD/YYYY"));
              } else {
                  $('#bgDate').text("");
              }
              $("#chartBloodGlucose").data("kendoChart").dataSource.data(data.userBloodGlucose);
              $('#currentBloodGlucose').text(data.Value);
          });

    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null,
            Last5: true
        }),
        url: "/User/BloodPressureChart",
        contentType: "application/json",
        dataType: 'json'
    })
     .done(function (data) {
         $("#chartBloodPressure").data("kendoChart").dataSource.data(data.userBloodPressure);
         $('#bloodPressure').text(data.Systolic + "/" + data.Diastolic);
         $('#heartRate').text(data.HeartRate);
         if (data.ResultDateTime != null) {
             $('#bpDate').text(moment(data.ResultDateTime).format("MM/DD/YYYY"));
         } else {
             $('#bpDate').text("");
         }
     });

    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null,
            Last5: true
        }),
        url: "/User/StepsChart",
        contentType: "application/json",
        dataType: 'json'
    })
      .done(function (data) {
          $("#chartSteps").data("kendoChart").dataSource.data(data.userActivity);
          $('#steps').text(data.Steps);
          if (data.StartDateTime != null) {
              $('#stepsDate').text(moment(data.StartDateTime).format("MM/DD/YYYY"));
          } else {
              $('#stepsDate').text("");
          }
      });

    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null,
            Last5: true
        }),
        url: "/User/WeightTrendChart",
        contentType: "application/json",
        dataType: 'json'
    })
     .done(function (data) {
         $("#chartWeightTrend").data("kendoChart").dataSource.data(data.userVital);
         $('#weight').text(Math.round(data.WeightLbs * 100) / 100);
         if (data.ResultDateTime != null) {
             $('#weightResultDate').text(moment(data.ResultDateTime).format("MM/DD/YYYY"));
         } else {
             $('#weightResultDate').text("");
         }
     });

    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null,
            Last5: true
        }),
        url: "/User/DietTrendChart",
        contentType: "application/json",
        dataType: 'json'
    })
     .done(function (data) {
         $("#chartDietTrend").data("kendoChart").dataSource.data(data.userDiet);
         $('#diet').text(data.Value);
         if (data.EnteredDateTime != null) {
             $('#dietDate').text(moment(data.EnteredDateTime).format("MM/DD/YYYY"));
         } else {
             $('#dietDate').text("");
         }
     });

    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null,
            Last5: true
        }),
        url: "/User/BodyCompositionChart",
        contentType: "application/json",
        dataType: 'json'
    })
     .done(function (data) {
         $("#chartBodyCompositionTrend").data("kendoChart").dataSource.data(data.userBodyComposition);
         $('#bodyFat').text(data.userBodyComposition[0].Value);
         $('#leanTissue').text(data.userBodyComposition[1].Value);
         $('#bodyDate').text(moment(data.ResultDateTime).format("MM/DD/YYYY"));

     });

    $.ajax({
        type: "POST",
        data: JSON.stringify({
            startDate: null,
            endDate: null,
            Last5: true
        }),
        url: "/User/CholestrolChart",
        contentType: "application/json",
        dataType: 'json'
    })
     .done(function (data) {
         $("#chartCholesterol").data("kendoChart").dataSource.data(data.userTest);
         $('#ldl').text(data.userResult.Ldl);
         $('#hdl').text(data.userResult.Hdl);
         $('#cholesterol').text(data.userResult.Cholesterol);
         $('#dateCholesterol').text(moment(data.userResult.ResultDateTime).format("MM/DD/YYYY"));

     });
};




function getParameterByName(name, url) {
    if (!url) url = window.location.href;
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
};













//    var SCOPE = 'https://www.googleapis.com/auth/plus.login';
//    var CLIENTID = '786250666458-8f6a9g4kl98i70t1colv1adurbunhdma.apps.googleusercontent.com';
//    var COOKIE_POLICY = 'single_host_origin';
//    var APPROVAL_PROMPT = 'force';
//    var ACCESS_TYPE = 'offline';
//    var CALLBACK = 'loginCallback';
//    var ACCESS_TOKEN;
//    var ID_TOKEN;
//    var EXPIRES_IN;




//    function onLoadCallback() {
//        //Load Google + API
//        gapi.client.load('plus', 'v1', function () { });        
//    }

//function googleSignup() {
//    var myParams = {
//        'clientid': CLIENTID, 
//        'cookiepolicy': COOKIE_POLICY,
//        'callback': CALLBACK, 
//        'approvalprompt': APPROVAL_PROMPT,
//        'accesstype':ACCESS_TYPE,
//        'scope': SCOPE,
//        immediate: true
//    };
//    gapi.auth.signIn(myParams);
//}
//function loginCallback(result) {
//    if (result['status']['signed_in']) {
//        ACCESS_TOKEN = result.access_token;
//        ID_TOKEN = result.id_token;
//        EXPIRES_IN = result.expires_in; 

//        var request = gapi.client.plus.people.get(
//{
//    'userId': 'me'
//});
//        request.execute(function (resp) {
//            //var email = '';
//            //if (resp['emails']) {
//            //    for (i = 0; i < resp['emails'].length; i++) {
//            //        if (resp['emails'][i]['type'] == 'account') {
//            //            email = resp['emails'][i]['value'];
//            //        }
//            //    }
//            //}
//            var modal = {
//                FirstName: resp.result.name.givenName,
//                LastName: resp.result.name.familyName,
//                Email: resp.result.emails[0].value,
//                InvitationCode: jQuery("#invitationCode").val(),
//                SocialUserId: resp.result.id,
//                SocialType: "GOOGLE",
//                SourceIdToken: ID_TOKEN,
//                AccessToken: ACCESS_TOKEN,
//                AccessTokenExpiration : EXPIRES_IN
//            };
//            $("#firstName").val(modal.FirstName);
//            $("#lastName").val(modal.LastName);
//            $("#signupEmail").val(modal.Email);
//            $("#signupConfirmEmail").val(modal.Email);
//            $('input:hidden[name=SocialUserId]').val(modal.SocialUserId);
//            $('input:hidden[name=SocialType]').val(modal.SocialType);
//            $('input:hidden[name=AccessToken]').val(modal.AccessToken);
//            $('input:hidden[name=PublicToken]').val(modal.PublicToken);
//            $("#invitationCode").val(modal.InvitationCode);
//        });
//    }

//}




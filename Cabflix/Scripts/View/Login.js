
$(document).ready(function () {
  $.validator.setDefaults({
    submitHandler: function () {
      alert( "Form successful submitted!" );
    }
  });
    $('#form-login').validate({
    rules: {
      Login: {
        required: true,
        email: true,
      },
      Senha: {
        required: true,
        minlength: 6
      },
    },
    messages: {
      email: {
        required: "Informe o Email!",
        email: "Informe um email válido!"
      },
      password: {
        required: "Informe a Senha!",
        minlength: "A Senha deve ter pelo menos 6 caracteres"
      },
    },
    errorElement: 'span',
    errorPlacement: function (error, element) {
      error.addClass('invalid-feedback');
      element.closest('.form-group').append(error);
    },
    highlight: function (element, errorClass, validClass) {
      $(element).addClass('is-invalid');
    },
    unhighlight: function (element, errorClass, validClass) {
      $(element).removeClass('is-invalid');
    }
  });
});

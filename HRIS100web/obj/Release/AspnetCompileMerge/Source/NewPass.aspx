<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="NewPass.aspx.vb" Inherits="AOS100web.NewPass" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title></title>
	<link rel="stylesheet" type="text/css" href="modal.css" />
	<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/4.1.2/css/bootstrap.min.css" />
	<link rel="stylesheet" href="https://use.fontawesome.com/releases/v5.3.1/css/all.css" />
	<script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.6.0/jquery.min.js" integrity="sha512-894YE6QWD5I59HgZOGReFYm4dnWc1Qt5NtvYSaNcOP+u1T9qYdvdihz0PPSiiqn/+/3e7Jo4EaG7TubfWGUrMQ==" crossorigin="anonymous" referrerpolicy="no-referrer"></script>
	<script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.14.3/umd/popper.min.js"></script>
	<script src="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/js/bootstrap.min.js"></script>
</head>
<body>
	<form id="form1" runat="server">

		<div class="container">
			<button type="button" class="btn btn-danger" data-toggle="modal" data-target="#form">
				See Modal with Form
			</button>
		</div>
		<div class="modal fade" id="form" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" style="display: none;">
			<div class="modal-dialog modal-dialog-centered" role="document">
				<div class="modal-content">
					<div class="modal-header border-bottom-0">
						<h5 class="modal-title" id="exampleModalLabel">Create Account</h5>
						<button type="button" class="close" data-dismiss="modal" aria-label="Close">
							<span aria-hidden="true">×</span>
						</button>
					</div>
					<form>
						<div class="modal-body">
							<div class="form-group">
								<label for="email1">Email address</label>
								<input type="email" class="form-control" id="email1" aria-describedby="emailHelp" placeholder="Enter email" />
								<small id="emailHelp" class="form-text text-muted">Your information is safe with us.</small>
							</div>
							<div class="form-group">
								<label for="password1">Password</label>
								<input type="password" class="form-control" id="password1" placeholder="Password" />
							</div>
							<div class="form-group">
								<label for="password1">Confirm Password</label>
								<input type="password" class="form-control" id="password2" placeholder="Confirm Password" />
							</div>
						</div>
						<div class="modal-footer border-top-0 d-flex justify-content-center">
							<button type="submit" class="btn btn-success">Submit</button>
						</div>
					</form>
				</div>
			</div>
		</div>

		<div>
		</div>
	</form>
</body>
</html>

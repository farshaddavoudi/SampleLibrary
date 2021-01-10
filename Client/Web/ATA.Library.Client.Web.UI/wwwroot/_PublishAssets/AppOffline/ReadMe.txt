
To make application offline:

	>> Rename _web.config file to web.config


	>> Copy app_offline.html and web.config files into project root (beside wwwroot)

		- Don't remove already web.config file, if you want to change the application back to online without another publish.
		- With new publish it will automatically will be replace by atuo generated web.config

	>> Customize offline message by manipulating app_offline.html with a text editor
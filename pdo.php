<?php
// PDO
function Conn(){
$conn = new PDO('mysql:host=localhost;port=3306;dbname=breakermind_9975139;charset=utf8', 'root', 'toor');
// don't cache query
$conn->setAttribute(PDO::ATTR_EMULATE_PREPARES, false);
// show warning text
$conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_WARNING);
// throw error exception
$conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
// don't colose connecion on script end
$conn->setAttribute(PDO::ATTR_PERSISTENT, false);
// set utf for conn utf8_general_ci or utf8_unicode_ci 
$conn->setAttribute(PDO::MYSQL_ATTR_INIT_COMMAND, "SET NAMES 'utf8mb4' COLLATE 'utf8mb4_unicode_ci'");
return $conn;
}

?>

<?php
header('Content-Type: text/html; charset=utf-8');
//header("Content-Type: application/json;charset=utf-8");
// allow use js from another host
//header('Access-Control-Allow-Origin: *');
//header("Access-Control-Allow-Credentials: true");
//header("Access-Control-Allow-Methods: GET, POST");
//header("Access-Control-Allow-Headers: Content-Type, *");
error_reporting('E_ALL');


// secure string
$nick = htmlentities($_GET['nick'], ENT_QUOTES, 'utf-8');
$pass = md5($_GET['pass']);

// return 1 if exists or 0 user does not exists
function is_user($nick, $pass, $db){
	$res = $db->query("SELECT * FROM users WHERE nick = '$nick' AND pass='$pass'");
	//$rows = $res->fetchAll(PDO::FETCH_ASSOC);
	$cnt = 0;
	$cnt = $res->rowCount();
	return $cnt;
}
function show_account($db){
	$res = $db->query("SELECT id, balance, equity, (SELECT COUNT(*)from account) as cnt FROM account ORDER BY id DESC LIMIT 10");
	$rows = $res->fetchAll(PDO::FETCH_ASSOC);	
	$data = array();
	$data['account'] = $rows;
	return json_encode($data);
}

function show_open($db){
	$res = $db->query("SELECT * FROM opensignal ORDER BY id DESC LIMIT 10");
	$rows = $res->fetchAll(PDO::FETCH_ASSOC);	
	$data = array();
	$data['open'] = $rows;
	return json_encode($data);
}

function show_close($db){
	$res = $db->query("SELECT * FROM closesignal ORDER BY id DESC LIMIT 10");
	$rows = $res->fetchAll(PDO::FETCH_ASSOC);	
	$data = array();
	$data['close'] = $rows;
	return json_encode($data);
}

// show errors
try{
	
	include('pdo.php');	
	// init db
	$db = Conn();
 	
 	echo "<h2> Show equity </h2>";
 	echo show_account($db);

 	echo "<h2> Open positions </h2>"; 	 		
 	echo show_open($db);

 	echo "<h2> Close positions </h2>"; 	 		
 	echo show_close($db);

} catch (Exception $e) {
    if ($e->getCode() == '2A000'){ echo "Syntax Error: ".$e->getMessage(); }
    //print_r($db->errorInfo());
  	//echo "PDO::errorCode(): ", $db->errorCode();
} 
?>

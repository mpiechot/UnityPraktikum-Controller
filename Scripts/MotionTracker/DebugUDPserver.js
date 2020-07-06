var udp = require('dgram');

// creating a udp server
var server = udp.createSocket('udp4');

//emits when socket is ready and listening for datagram msgs
server.on('listening',function(){
  var address = server.address();
  var port = address.port;
  var family = address.family;
  var ipaddr = address.address;
  console.log('Server is listening at port: ' + port);
  console.log('Server ip: ' + ipaddr);
  console.log('Server is IP4/IP6: ' + family);
});

// reacts on incoming messages
// server.on('message',function(msg,info){
//   console.log('Data received from client : ' + msg.toString());
//   console.log('Received %d bytes from %s:%d\n',msg.length, info.address, info.port);


server.bind(12346);



// example which simulates the hand positions
let x = 3.5;
let y = 0;
let z = 0;

// remote_port (which identifies the udp client running besides unity)
let remote_port = 12345;

setInterval(function(){
  //Every frame, simply move it to the left until it reaches the center
  x = Math.max(x - 0.05,0);

  //construct a msg format
  let msg = x + "," + y + "," + z;

  //send message
  server.send(msg,remote_port,'localhost',function(error){
    if(error){
      console.log("failed to send data");
      client.close();
    }else{
      console.log(msg);
    }
  });
},100)


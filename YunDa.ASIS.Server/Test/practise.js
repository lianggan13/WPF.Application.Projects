
db.DP_User.update({},{"$unset":{"Roles":""}},{multi:true})
db.DP_User.update({},{"$unset":{"Role":""}},{multi:true})

db.c1.insertOne({"name":"lianggan13"})

user1=  {
	"name":"lianggan13",
	"age":18,
	"hobbies":["football","running"],
	"addres":{
		"country":"China",
		"city":"CD"
	}
}

user2 =  {
	"_id":13, // 指定 _id
	"name":"lianggan13",
	"age":18,
	"hobbies":["football","running"],
	"addres":{
		"country":"China",
		"city":"CD"
	}
}


db.user.insert(user1)


db.user.insert(user2)

// _id 存在 --> update, 不存在 --> insert
db.user.save(user2)

db.user.insert([user1,user1,user1])
db.user.insertMany([user1,user1,user1])



user3 =  {
	"name":"sdfsdfsd",
	"age":25,
	"hobbies":["eating","sleeping"],
	"addres":{
		"country":"China",
		"city":"CD"
	}
}

// db.user.updateOne(filter, update, options)
// filter --> where
db.user.updateOne({"_id":13},{"$set":user3})

// db.user.update(query, update, options)
db.user.update({"_id":13},user3) // 默认修改第一条
db.user.update({"_id":13},{"$set":user3},false,true) // 修改多条
db.user.updateMany({"_id":13}, {"$set":user3}) // 修改多条
db.user.updateMany({"name":"lianggan13"},{$inc:{age:+7}},true,true); // 年龄 +7 

db.user.deleteOne({"name":"sdfsdfsd"})
db.user.remove({"name":"sdfsdfsd"}, {justOne:true})
db.user.deleteMany({})
db.user.remove({})
db.user.deleteMany({"addres.city":"CD"})
db.user.deleteMany({"age":{"$gte":18}})

db.user.find()
// db.user.distinct(field, query, options)
db.user.distinct("name")
db.user.find().pretty()
// != ('$ne")	> ('$gt')		< ('$lt')  >= ('$gte")	<= ('$lte")
db.user.find({"age":{"$gte":18}})
db.user.find({"name":"lianggan13","age":18})
db.user.find({"$or":[{"name":"lianggan13"},{"age":18}]})
// where age>=18 and (name == 'lianggan13' || age == 'CD')
db.user.find({"age":{"$gte":18},"$or":[{"name":"lianggan13"},{"addres.city":"CD"}]})
db.user.find({"age" : {$type : 'double'}})
db.user.find({},{"name":1,"addres":2}).skip(1).limit(2)
db.user.find({}).sor("age":-1) // -1: 降序 1: 升序

// db.user.aggregate(pipeline, options)
db.user.aggregate([{$group:{_id:"$name", count:{$sum:1}}}]) // 根据 name 统计人数  select name, count(*) from user group by name
$avg	$min	$max



db.user.aggregate(
{
  $project:{
		_id:0,
		name:1,
		age:1,
	}
});

db.user.aggregate([
    {
        $group: {
            _id: "$name",
            count: {
                $sum: 1
            },
            total: {
                $sum: "$num"
            },
		}}
]);

db.user.aggregate(
	[
		{$match:{"age":{$gte:18,$lte:18}}},
		{$group:{_id:"$name","count":{$sum:1}}}
	]
)

db.user.aggregate({$skip:2})


db.DP_User.aggregate([
    {
        $group: {
            _id: "$name",
            count: {
                $sum: 1
            },
            total: {
                $sum: "$No" 
            },
		}}
]);

db.DP_User.aggregate([
    {
        $match: {
            CardNo: 0
        }
    },
    {
        $group: {
            _id: "$RoleId",
            count: {
                $sum: 1
            }
        }
    },
		{
        $match: {
            count: 2
        }
    }
]);


// ----------------------------
// View structure for DP_TrackView
// ----------------------------
db.getCollection("DP_TrackView").drop();
db.createView("DP_TrackView","DP_Track",[
    {
        $lookup: {
            from: "DP_PositionView",
            localField: "PositionIds",
            foreignField: "_id",
            as: "Positions"
        }
    },
    {
        $project: {
            PositionIds: 0
        }
    }
]);


// DP_User --> DP_Role
db.DP_User.aggregate([
	{
		$lookup:{
			from: "DP_Role",
			localField:"RoleId",
			foreignField:"_id",
			as: "Role"
		},
	}
])

// $lookup 的时候会搭配使用 $match 和 $project

createView("DP_TrackView","DP_Track",[
    {
        $lookup: {
            from: "DP_PositionView",
            localField: "PositionIds",
            foreignField: "_id",
            as: "Positions"
        }
    },
    {
        $project: {
            PositionIds: 0
        }
    }
]);

{aggregate([{ "$lookup" : { "from" : "DP_Role", "localField" : "RoleId", "foreignField" : "_id", "as" : "Role" } }])}

db.DP_User.aggregate([{
    "$lookup": {
        "from": "DP_Role",
        "localField": "RoleId",
        "foreignField": "_id",
        "as": "Role"
    }
}])

// $lookup 的时候会搭配使用 $match 和 $project
// match相当于SQL中的WHERE，project 相当于 SQL 中 SELECT 后面的内容，1 为显示，0 为隐藏
db.student.aggregate([{  
	$lookup: {  
	from: "class",  
	localField: "class_id",  
	foreignField: "id",  
	as: "class_info"  
	}  
	}, {  
	$match: {  
	"age": {  
	$gt: 12  
	}  
	}  
	}, {  
	$project: {  
	"_id": 0,  
	"name": 1,  
	"age": 1,  
	"class_info._id": 1  
	}  
}])  

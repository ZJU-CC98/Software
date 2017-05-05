using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CC98.Software.Data
{
	/// <summary>
	/// 软件站数据库上下文对象。
	/// </summary>
	public class SoftwareDbContext : DbContext
	{

		#region 构造方法

		// 请勿删除下列方法

		/// <summary>
		/// 用指定的数据库上下文创建一个 <see cref="SoftwareDbContext"/> 对象的新实例。
		/// </summary>
		/// <param name="options">数据库上下文信息，包含了数据库的链接方式和其它必要配置。</param>
		public SoftwareDbContext(DbContextOptions<SoftwareDbContext> options) : base(options)
		{
		}

		#endregion

	    public virtual DbSet<Software> Softwares { get; set; }
	    public virtual DbSet<Category> Categories  { get; set; }
	    public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

	}
}
